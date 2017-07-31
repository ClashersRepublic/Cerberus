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
        internal Pool<SocketAsyncEventArgs> ArgsPool;
        internal Pool<byte[]> BufferPool;
        internal Socket Listener;
        internal int ConnectedSockets;
        internal int NumberOfBuffers => BufferPool.Count;
        internal int NumberOfArgs => ArgsPool.Count;

        public Gateway()
        {
            ArgsPool = new Pool<SocketAsyncEventArgs>();
            BufferPool = new Pool<byte[]>();

            Initialize();

            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Listener.Bind(new IPEndPoint(IPAddress.Any, 9339));
            Listener.Listen(100);

            Program.Stopwatch.Stop();

            Loggers.Log(
                Assembly.GetExecutingAssembly().GetName().Name +
                $" has been started on {Utils.LocalNetworkIP} in {Math.Round(Program.Stopwatch.Elapsed.TotalSeconds, 4)} Seconds!",
                true
            );

            var args = GetArgs();
            StartAccept(args);
        }

        internal void Initialize()
        {
            for (int i = 0; i < Constants.PRE_ALLOC_SEA; i++)
            {
                var args = new SocketAsyncEventArgs();
                args.Completed += OnIOCompleted;

                ArgsPool.Push(args);
            }
        }

        internal void StartAccept(SocketAsyncEventArgs e)
        {
            try
            {
                while (true)
                {
                    if (!Listener.AcceptAsync(e))
                        ProcessAccept(e, false);
                    else
                        break;
                }
            }
            catch (Exception ex)
            {
                Resources.Exceptions.Catch(ex, "Exception while starting to accept(critical)");
            }
        }

        internal void StartReceive(SocketAsyncEventArgs e)
        {
            var token = (Token) e.UserToken;
            var socket = token.Device.Socket;
            try
            {
                while (true)
                {
                    if (!socket.ReceiveAsync(e))
                        ProcessReceive(e, false);
                    else
                        break;
                }
            }
            catch (ObjectDisposedException)
            {
                Recycle(e);
            }
            catch (Exception ex)
            {
                Resources.Exceptions.Catch(ex, "Exception while start receive: ");
            }
        }

        internal void ProcessAccept(SocketAsyncEventArgs e, bool startNew)
        {
            var socket = e.AcceptSocket;

            if (e.SocketError == SocketError.Success)
            {
                if (Constants.Local)
                {
                    if (!Constants.AuthorizedIP.Contains(socket.RemoteEndPoint.ToString().Split(':')[0]))
                    {
                        socket.Close();
                        e.AcceptSocket = null;
                        if (startNew)
                            StartAccept(e);
                        return;
                    }
                }

                Loggers.Log($"New client connected -> {socket.RemoteEndPoint}", true);

                var args = GetArgs();
                var buffer = GetBuffer;

                DefensiveSetBuffer(ref args, buffer);

                var device = new Device(socket);
                var token = new Token(args, device);

                Interlocked.Increment(ref ConnectedSockets);
                Resources.Devices.Add(device);

                StartReceive(args);
            }
            else
            {
                Loggers.Log("Not connected or error at ProcessAccept.", false, Defcon.ERROR);
                KillSocket(socket);
            }

            // Clean shit up for reuse.
            e.AcceptSocket = null;

            if (startNew)
                StartAccept(e);
        }

        internal void ProcessReceive(SocketAsyncEventArgs e, bool startNew)
        {
            var transferred = e.BytesTransferred;
            if (transferred == 0 || e.SocketError != SocketError.Success)
            {
                Disconnect(e);
                Recycle(e);
            }
            else
            {
                Token token = (Token)e.UserToken;

                var buffer = e.Buffer;
                var offset = e.Offset;
                for (int i = 0; i < transferred; i++)
                    token.Packet.Add(buffer[offset + i]);

                try
                {
                    token.Process();
                }
                catch (Exception ex)
                {
                    Resources.Exceptions.Catch(ex, "Exception while processing receive");
                }

                if (startNew)
                    StartReceive(e);
            }
        }

        internal void Disconnect(SocketAsyncEventArgs e)
        {
            var token = (Token)e.UserToken;

            if (token.Device.Player != null)
            {
                if (Resources.Players.ContainsKey(token.Device.Player.Avatar.UserId))
                    Resources.Players.Remove(token.Device.Player);
            }
            else
            {
                Resources.Devices.Remove(token.Device.SocketHandle);
            }

            Interlocked.Decrement(ref ConnectedSockets);
        }

        internal void Send(Message message)
        {
            var args = GetArgs();
            var buffer = default(byte[]);

            try
            {
                buffer = message.ToBytes;
            }
            catch (Exception ex)
            {
                Resources.Exceptions.Catch(ex, $"Exception while constructing message {message.GetType()}");
                return;
            }

            DefensiveSetBuffer(ref args, buffer);
            args.UserToken = message.Device.Token;

            StartSend(args);
        }

        internal void StartSend(SocketAsyncEventArgs e)
        {
            var token = (Token)e.UserToken;
            var socket = token.Device.Socket;
            try
            {
                while (true)
                {
                    if (!socket.SendAsync(e))
                        ProcessSend(e);
                    else
                        break;
                }
            }
            catch (ObjectDisposedException)
            {
                Recycle(e);
            }
            catch (Exception ex)
            {
                Resources.Exceptions.Catch(ex, "Exception while starting send");
            }
        }

        internal void ProcessSend(SocketAsyncEventArgs e)
        {
            var transferred = e.BytesTransferred;
            if (transferred == 0 || e.SocketError != SocketError.Success)
            {
                Disconnect(e);
                Recycle(e);
            }
            else
            {
                try
                {
                    var offset = e.Offset;
                    var count = e.Count;
                    if (transferred < count)
                    {
                        e.SetBuffer(e.Offset + transferred, count - transferred);
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
                    Resources.Exceptions.Catch(ex, "Exception while processing send");
                }
            }
        }

        internal void OnIOCompleted(object sender, SocketAsyncEventArgs e)
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
                    throw new ArgumentException("The last operation completed on the socket was not a receive, send or accept");
            }
        }

        internal void Recycle(SocketAsyncEventArgs e)
        {
            if (e == null)
                return;

            var buffer = e.Buffer;
            e.UserToken = null;
            e.SetBuffer(null, 0, 0);
            e.AcceptSocket = null;

            ArgsPool.Push(e);

            Recycle(buffer);
        }

        internal void Recycle(byte[] buffer)
        {
            if (buffer?.Length == Constants.ReceiveBuffer)
                BufferPool.Push(buffer);
        }

        internal void KillSocket(Socket socket)
        {
            if (socket == null)
                return;

            try { socket.Disconnect(false); }
            catch { /* Swallow */ }
            try { socket.Close(5); }
            catch { /* Swallow */ }
            try { socket.Dispose(); }
            catch { /* SWallow */ }
        }

        internal void DefensiveSetBuffer(ref SocketAsyncEventArgs args, byte[] buffer)
        {
            try
            {
                args.SetBuffer(buffer, 0, buffer.Length);
            }
            catch (InvalidOperationException)
            {
                //Logger.SayInfo($"A SocketAsynceEvenArgs object was already in use. Last Op => {args.LastOperation}.");

                args = new SocketAsyncEventArgs();
                args.Completed += OnIOCompleted;
                args.SetBuffer(buffer, 0, buffer.Length);
            }
        }

        internal SocketAsyncEventArgs GetArgs()
        {
            var args = ArgsPool.Pop();
            if (args == null)
            {
                //Logger.SayInfo("Creating new SocketAsyncEventArgs object since pool was empty(returned null).");
                args = new SocketAsyncEventArgs();
                args.Completed += OnIOCompleted;
            }
            return args;
        }

        internal byte[] GetBuffer => BufferPool.Pop() ?? new byte[Constants.ReceiveBuffer];
    }
}
