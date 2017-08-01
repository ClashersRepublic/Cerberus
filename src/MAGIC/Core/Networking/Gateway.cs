using CRepublic.Magic.Extensions;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Packets;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;

namespace CRepublic.Magic.Core.Networking
{
    internal class Gateway
    {
        internal static Pool<SocketAsyncEventArgs> ArgsPool;
        internal static Pool<byte[]> BufferPool;
        internal static Socket Listener;

        internal static int ConnectedSockets;


        internal static int NumberOfBuffers => BufferPool.Count;
        internal static int NumberOfBuffersCreated => BuffersCreated;
        internal static int NumberOfBuffersInUse => BuffersCreated - BufferPool.Count;

        internal static int NumberOfArgs => ArgsPool.Count;
        internal static int NumberOfArgsCreated => ArgsCreated;
        internal static int NumberOfArgsInUse => ArgsCreated - ArgsPool.Count;

        private static int BuffersCreated;
        private static int ArgsCreated;
        private static Semaphore Semaphore;


        internal static void Listen()
        {
            Listener.Bind(new IPEndPoint(IPAddress.Any, 9339));
            Listener.Listen(100);

            Program.Stopwatch.Stop();

            Loggers.Log(Assembly.GetExecutingAssembly().GetName().Name + $" has been started on {Utils.LocalNetworkIP} in {Math.Round(Program.Stopwatch.Elapsed.TotalSeconds, 4)} Seconds!", true);

            var args = GetArgs();
            StartAccept(args);
        }

        internal static void Initialize()
        {
            var numThreads = Environment.ProcessorCount * 2;
            Semaphore = new Semaphore(numThreads, numThreads);
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ArgsPool = new Pool<SocketAsyncEventArgs>();

            for (int i = 0; i < Constants.PRE_ALLOC_SEA; i++)
            {
                var args = new SocketAsyncEventArgs();
                args.Completed += OnIOCompleted;

                ArgsPool.Push(args);
                ArgsCreated++;
            }

            BufferPool = new Pool<byte[]>();
        }

        internal static void StartAccept(SocketAsyncEventArgs e)
        {
            try
            {
                while (!Listener.AcceptAsync(e))
                {
                    ProcessAccept(e, false);
                }
            }
            catch (Exception ex)
            {
                Exceptions.Log(ex, "Exception while starting to accept(critical)");
            }
        }

        internal static void StartReceive(SocketAsyncEventArgs e)
        {
            var token = (Device) e.UserToken;
            var socket = token.Socket;
            try
            {
                while (!socket.ReceiveAsync(e))
                {
                    ProcessReceive(e, false);
                }
            }
            catch (ObjectDisposedException)
            {
                Drop(e);
            }
            catch (Exception ex)
            {
                Exceptions.Log(ex, "Exception while start receive: ");
            }
        }

        internal static void ProcessAccept(SocketAsyncEventArgs e, bool startNew)
        {
            var socket = e.AcceptSocket;

            if (e.SocketError == SocketError.Success)
            {
                /*if (Constants.Local)
                {
                    if (!Constants.AuthorizedIP.Contains(socket.RemoteEndPoint.ToString().Split(':')[0]))
                    {
                        socket.Close();
                        e.AcceptSocket = null;
                        if (startNew)
                            StartAccept(e);
                        return;
                    }
                }*/

                //Loggers.Log($"New client connected -> {socket.RemoteEndPoint}", true);

                var device = new Device(socket);
                Devices.Add(device);

                var args = GetArgs();
                var buffer = GetBuffer();

                args.UserToken = device;
                args.SetBuffer(buffer, 0, buffer.Length);

                StartReceive(args);
            }
            else
            {
                Loggers.Log("Not connected or error at ProcessAccept.", false, Defcon.ERROR);
                Drop(e);
                e = GetArgs();
            }

            // Clean shit up for reuse.
            e.AcceptSocket = null;

            if (startNew)
                StartAccept(e);
        }

        internal static void ProcessReceive(SocketAsyncEventArgs e, bool startNew)
        {
            var transferred = e.BytesTransferred;
            if (transferred == 0 || e.SocketError != SocketError.Success)
            {
                Drop(e);
            }
            else
            {
                Device token = (Device) e.UserToken;

                var buffer = e.Buffer;
                var offset = e.Offset;
                for (int i = 0; i < transferred; i++)
                    token.Stream.Add(buffer[offset + i]);

                var level = token.Player;
                var message = default(Message);
                while (token.TryGetPacket(out message))
                {
                    try { message.Process(); }
                    catch (Exception ex)
                    {
                        //ExceptionLogger.Log(ex, $"Exception while processing incoming message {message.GetType()}");
                    }
                }
                /*
                try
                {
                    token.Process();
                }
                catch (Exception ex)
                {
                    Exceptions.Log(ex, "Exception while processing receive");
                }*/

                if (startNew)
                    StartReceive(e);
            }
        }

        internal static void Disconnect(SocketAsyncEventArgs e)
        {
            var token = (Device) e.UserToken;

            if (token.Player != null)
            {
                if (Players.Levels.ContainsKey(token.Player.Avatar.UserId))
                    Players.Remove(token.Player);
            }
            else
            {
                Devices.Remove(token.SocketHandle);
            }

            Interlocked.Decrement(ref ConnectedSockets);
        }

        internal static void Send(Message message)
        {
            var args = GetArgs();
            var buffer = default(byte[]);

            try
            {
                buffer = message.ToBytes;
            }
            catch (Exception ex)
            {
                Exceptions.Log(ex, $"Exception while constructing message {message.GetType()}");
                return;
            }

            args.SetBuffer(buffer, 0, buffer.Length);
            args.UserToken = message.Device;

            StartSend(args);
        }

        internal static void StartSend(SocketAsyncEventArgs e)
        {
            var token = (Device) e.UserToken;
            var socket = token.Socket;
            try
            {
                while (!socket.SendAsync(e))
                    ProcessSend(e);
            }
            catch (ObjectDisposedException)
            {
                Drop(e);
            }
            catch (Exception ex)
            {
                Exceptions.Log(ex, "Exception while starting send");
            }
        }

        internal static void ProcessSend(SocketAsyncEventArgs e)
        {
            var transferred = e.BytesTransferred;
            if (transferred == 0 || e.SocketError != SocketError.Success)
            {
                Drop(e);
            }
            else
            {
                try
                {
                    var offset = e.Offset;
                    var count = e.Count;
                    if (transferred < count)
                    {
                        e.SetBuffer(offset + transferred, count - transferred);
                        StartSend(e);
                    }
                    else
                    {
                        // We done with sending can recycle EventArgs.
                        Recycle(e);
                    }
                }
                catch (Exception ex)
                {
                    Exceptions.Log(ex, "Exception while processing send");
                }
            }
        }

        internal static void OnIOCompleted(object sender, SocketAsyncEventArgs e)
        {
            const int TIME_OUT = 10000;
            var semaphoreAcquired = false;
            try
            {
                semaphoreAcquired = Semaphore.WaitOne(TIME_OUT);
                if (semaphoreAcquired)
                {
                    if (e.SocketError == SocketError.Success)
                    {
                        switch (e.LastOperation)
                        {
                            case SocketAsyncOperation.Accept:
                                ProcessAccept(e, true);
                                break;
                            case SocketAsyncOperation.Receive:
                                ProcessReceive(e, true);
                                break;
                            case SocketAsyncOperation.Send:
                                ProcessSend(e);
                                break;
                            default:
                                throw new Exception("Unexpected operation.");
                        }
                    }

                    else
                    {
                        Loggers.Log($"A socket operation wasn't successful => {e.LastOperation}. Dropping connection.", true);
                        Drop(e);

                        // If the last operation was an accept operation, continue accepting
                        // for new connections.
                        if (e.LastOperation == SocketAsyncOperation.Accept)
                        {
                            var args = GetArgs();
                            StartAccept(args);
                        }
                    }
                }
                else
                {
                    Loggers.Log($"SEMAPHORE DID NOT RESPOND IN TIME!", true);
                }
            }
            catch (Exception ex)
            {
                Exceptions.Log(ex,
                    "Exception occurred while processing async operation(potentially critical). Dropping connection");
                Loggers.Log(
                    ex +
                    "Exception occurred while processing async operation(potentially critical). Dropping connection",
                    true);
                Drop(e);
            }
            finally
            {
                if (semaphoreAcquired)
                    Semaphore.Release();
            }
        }

        internal static void Drop(SocketAsyncEventArgs e)
        {
            if (e == null)
                return;

            var client = e.UserToken as Device;
            if (client != null)
            {
                // If the resource manager did not mange to kill the socket, we do it here.
                if (!Devices.Remove(client.SocketHandle))
                    KillSocket(client.Socket);
            }
            else if (e.LastOperation == SocketAsyncOperation.Accept)
            {
                KillSocket(e.AcceptSocket);
            }

            // Recycle the object along with its buffer.
            Recycle(e);
        }

        internal static void Recycle(SocketAsyncEventArgs e)
        {
            if (e == null)
                return;

            var buffer = e.Buffer;
            e.UserToken = null;
            e.SetBuffer(null, 0, 0);
            e.AcceptSocket = null;

            Recycle(buffer);

            ArgsPool.Push(e);
        }

        internal static  void Recycle(byte[] buffer)
        {
            if (buffer?.Length == Constants.ReceiveBuffer)
                BufferPool.Push(buffer);
        }

        internal static void KillSocket(Socket socket)
        {
            if (socket == null)
                return;

            try
            {
                socket.Disconnect(false);
            }
            catch
            {
                /* Swallow */
            }
            try
            {
                socket.Close(5);
            }
            catch
            {
                /* Swallow */
            }
            try
            {
                socket.Dispose();
            }
            catch
            {
                /* SWallow */
            }
        }

        internal static SocketAsyncEventArgs GetArgs()
        {
            var args = ArgsPool.Pop();
            if (args == null)
            {
                //Logger.SayInfo("Creating new SocketAsyncEventArgs object since pool was empty(returned null).");
                args = new SocketAsyncEventArgs();
                args.Completed += OnIOCompleted;

                Interlocked.Increment(ref ArgsCreated);
            }
            return args;
        }

        internal static byte[] GetBuffer()
        {
            var buffer = BufferPool.Pop();
            if (buffer == null)
            {
                buffer = new byte[Constants.ReceiveBuffer];

                Interlocked.Increment(ref BuffersCreated);
            }
            return buffer;
        }

    }
}
