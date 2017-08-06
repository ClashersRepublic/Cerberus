using CRepublic.Magic.Extensions;
using CRepublic.Magic.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using CRepublic.Magic.Core.Interface;
using CRepublic.Magic.Core.Resource;
using CRepublic.Magic.Extensions.List;
using CRepublic.Magic.Packets;

namespace CRepublic.Magic.Core.Network
{
    internal static class Gateway
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

            BufferPool = new Pool<byte[]>
            {
                Limit = 2000
            };
        }

        internal static void Listen()
        {
            Listener.Bind(new IPEndPoint(IPAddress.Any, 9339));
            Listener.Listen(100);
            Listener.ReceiveBufferSize = Constants.Buffer;
            Listener.SendBufferSize = Constants.Buffer;

            var args = GetArgs();
            StartAccept(args);  

            Program.Stopwatch.Stop();
            Control.Say(Assembly.GetExecutingAssembly().GetName().Name + $" has been started on {Utils.LocalNetworkIP} in {Math.Round(Program.Stopwatch.Elapsed.TotalMilliseconds, 5)} Milliseconds!");
        }

        internal static void StartAccept(SocketAsyncEventArgs e)
        {
            try
            {
                if (!Listener.AcceptAsync(e))
                    ProcessAccept(e, false);
            }
            catch (Exception ex)
            {
                //Exceptions.Log(ex, "Exception while starting to accept(critical)");
            }
        }

        internal static void ProcessAccept(SocketAsyncEventArgs e, bool startNew)
        {
            var acceptSocket = e.AcceptSocket;

            if (e.SocketError != SocketError.Success)
            {
                //Loggers.Log("Not connected or error at ProcessAccept.", false, Defcon.ERROR);
                Drop(e);

                e = GetArgs();
            }
            else
            {
                try
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

                    //if (Constants.Verbosity > 3)
                        //Loggers.Log($"New client connected -> {socket.RemoteEndPoint}", true);
                        //Console.WriteLine($"New client connected -> {acceptSocket.RemoteEndPoint}");
                    var device = new Device(acceptSocket);
                    Devices.Add(device);

                    var args = GetArgs();
                    var buffer = GetBuffer();

                    args.UserToken = device;
                    DefensiveSetBuffer(ref args, buffer);

                    StartReceive(args);
                }
                catch (Exception ex)
                {
                    //Exceptions.Log(ex, "Exception while processing accept");
                }
            }

            // Clean shit up for reuse.
            e.AcceptSocket = null;

            if (startNew)
                StartAccept(e);
        }


        internal static void StartReceive(SocketAsyncEventArgs e)
        {
            var device = (Device) e.UserToken;
            var socket = device.Socket;

            try
            {
                if (!socket.ReceiveAsync(e))
                    ProcessReceive(e, false);
            }
            catch (ObjectDisposedException)
            {
                Drop(e);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception while start receive: ");
            }
        }

        internal static void ProcessReceive(SocketAsyncEventArgs e, bool startNew)
        {
            var device = (Device) e.UserToken;
            var transferred = e.BytesTransferred;
            if (transferred == 0 || e.SocketError != SocketError.Success)
            {
                Drop(e);
            }
            else
            {
                try
                {
                    var buffer = e.Buffer;
                    var offset = e.Offset;
                    for (int i = 0; i < transferred; i++)
                        device.Stream.Add(buffer[offset + i]);

                    var message = default(Message);

                    while (device.TryGetPacket(out message))
                    {
                        try
                        {
                            message.Process();
                        }
                        catch (Exception ex)
                        {
                            //Exceptions.Log(ex, $"Exception while processing incoming message {message.GetType()}");
                        }
                    }

                }
                catch (Exception ex)
                {
                    //Exceptions.Log(ex, "Exception while processing receive");
                }

                if (startNew)
                    StartReceive(e);
            }
        }

        internal static void Send(this Message message)
        {
            var buffer = default(byte[]);
            try
            {
                message.Encode();
            }
            catch (Exception ex)
            {
                //Exceptions.Log(ex, $"Exception while encoding message {Message.GetType()}");
                return;
            }

            try
            {
                message.Encrypt();
            }
            catch (Exception ex)
            {
                //Exceptions.Log(ex, $"Exception while encrypting message {Message.GetType()}");
                return;
            }

            try
            {
                buffer = message.ToBytes;
            }
            catch (Exception ex)
            {
                //Exceptions.Log(ex, $"Exception while constructing message {message.GetType()}");
                return;
            }


            try
            {
                message.Process();
            }
            catch (Exception ex)
            {
                //Exceptions.Log(ex, $"Exception while processing message {message.GetType()}");
                return;
            }

            var args = GetArgs();
            DefensiveSetBuffer(ref args, buffer);
            args.UserToken = message.Device;

            StartSend(args);
        }

        internal static void StartSend(SocketAsyncEventArgs e)
        {
            var token = (Device) e.UserToken;
            var socket = token.Socket;

            try
            {
                if (!socket.SendAsync(e))
                    ProcessSend(e);
            }
            catch (ObjectDisposedException)
            {
                Drop(e);
            }
            catch (Exception ex)
            {
                //Exceptions.Log(ex, "Exception while starting send");
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
                    //Exceptions.Log(ex, "Exception while processing send");
                }
            }
        }

        internal static void Disconnect(SocketAsyncEventArgs e)
        {
            var token = (Device)e.UserToken;

            /*if (token.Player != null)
            {
                if (Players.Levels.ContainsKey(token.Player.Avatar.UserId))
                    Players.Remove(token.Player);
            }
            else
            {
                Devices.Remove(token.SocketHandle);
            }
            */
            Interlocked.Decrement(ref ConnectedSockets);
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
                        Control.Say($"A socket operation wasn't successful => {e.LastOperation}. Dropping connection.");
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
                    Control.Say($"SEMAPHORE DID NOT RESPOND IN TIME!");
                }
            }
            catch (Exception ex)
            {
                Control.Say(ex + "Exception occurred while processing async operation(potentially critical). Dropping connection");
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
            DefensiveSetBuffer(ref e, null);
            e.AcceptSocket = null;

            Recycle(buffer);
            buffer = null;

            ArgsPool.Push(e);
        }

        internal static void Recycle(byte[] buffer)
        {
            if (buffer?.Length == Constants.Buffer)
            {
                BufferPool.Push(buffer);
            }
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
                //Control.SayInfo("Creating new SocketAsyncEventArgs object since pool was empty(returned null).");
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
                buffer = new byte[Constants.Buffer];

                Interlocked.Increment(ref BuffersCreated);
            }
            return buffer;
        }

        internal static void DefensiveSetBuffer(ref SocketAsyncEventArgs args, byte[] buffer)
        {
            try
            {
                args.SetBuffer(buffer, 0, buffer?.Length ?? 0 );
            }
            catch (InvalidOperationException)
            {
                Control.SayInfo($"A SocketAsynceEvenArgs object was already in use. Last Op => {args.LastOperation}.");

                args = new SocketAsyncEventArgs();
                args.Completed += OnIOCompleted;
                args.SetBuffer(buffer, 0, buffer?.Length ?? 0);
            }
        }

    }
}
