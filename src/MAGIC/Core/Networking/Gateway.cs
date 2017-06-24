using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using CRepublic.Magic.Packets;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;
using SharpRaven.Data;

namespace CRepublic.Magic.Core.Networking
{
    internal class Gateway
    {
        internal Pool<SocketAsyncEventArgs> ArgsPool;
        internal Pool<byte[]> BufferPool;
        internal Socket Listener;
        //internal Mutex Mutex;
        internal int ConnectedSockets;

        internal Gateway()
        {
            this.ArgsPool = new Pool<SocketAsyncEventArgs>();
            this.BufferPool = new Pool<byte[]>();

            this.Initialize();

            this.Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);    

            this.Listener.Bind(new IPEndPoint(IPAddress.Any, 9339));
            this.Listener.Listen(100);

            Program.Stopwatch.Stop();

            Loggers.Log(
                Assembly.GetExecutingAssembly().GetName().Name +
                $" has been started on {Utils.LocalNetworkIP} in {Math.Round(Program.Stopwatch.Elapsed.TotalSeconds, 4)} Seconds!",
                true);

            var args = this.GetArgs();
            this.StartAccept(args);
        }

        internal void Initialize()
        {
            for (int i = 0; i < Constants.PRE_ALLOC_SEA; i++)
            {
                var Event = new SocketAsyncEventArgs();
                Event.Completed += this.OnIOCompleted;
                this.ArgsPool.Push(Event);
            }
        }

        internal void StartAccept(SocketAsyncEventArgs e)
        {
            try
            {
                while (true)
                {
                    if (!this.Listener.AcceptAsync(e))
                        this.ProcessAccept(e, false);
                    else
                        break;
                }
            }
            catch (Exception ex)
            {
                Resources.Exceptions.Catch(ex, "Exception while starting to accept(critical)");
            }
        }

        internal void StartReceive(SocketAsyncEventArgs AsyncEvent)
        {
            var client = (Token)AsyncEvent.UserToken;
            var socket = client.Device.Socket;

            if (Thread.VolatileRead(ref client.Device.Dropped) == 1)
            {
                this.Recycle(AsyncEvent);
            }
            else
            {
                try
                {
                    while (true)
                    {
                        if (!socket.ReceiveAsync(AsyncEvent))
                            this.ProcessReceive(AsyncEvent, false);
                        else
                            break;
                    }
                }
                catch (ObjectDisposedException)
                {
                    this.Recycle(AsyncEvent);
                }
                catch (Exception ex)
                {
                    Resources.Exceptions.Catch(ex, "Exception while start receive: ");
                }
            }
        }

        internal void ProcessAccept(SocketAsyncEventArgs AsyncEvent, bool startNew)
        {
            Socket Socket = AsyncEvent.AcceptSocket;

            if (AsyncEvent.SocketError == SocketError.Success)
            {
                if (Constants.Local)
                {
                    if (!Constants.AuthorizedIP.Contains(Socket.RemoteEndPoint.ToString().Split(':')[0]))
                    {
                        Socket.Close();
                        AsyncEvent.AcceptSocket = null;
                        if (startNew)
                            StartAccept(AsyncEvent);
                        return;
                    }
                }

                
                Loggers.Log($"New client connected -> {((IPEndPoint) Socket.RemoteEndPoint)}:", true);

                var Event = GetArgs();
                var buffer = GetBuffer;

                Event.AcceptSocket = Socket;
                Event.SetBuffer(buffer, 0, buffer.Length);

                Device device = new Device(Socket)
                {
                    IPAddress = ((IPEndPoint) Socket.RemoteEndPoint).Address.ToString()
                };

                Token Token = new Token(Event, device);
                device.Token = Token;

                Interlocked.Increment(ref this.ConnectedSockets);
                Resources.Devices.Add(device);

                this.StartReceive(Event);

            }
            else
            {
                Loggers.Log("Not connected or error at ProcessAccept.", false, Defcon.ERROR);
                this.KillSocket(Socket);
            }

            AsyncEvent.AcceptSocket = null;
            if (startNew)
                StartAccept(AsyncEvent);
        }

        internal void ProcessReceive(SocketAsyncEventArgs AsyncEvent, bool startNew)
        {
            var transferred = AsyncEvent.BytesTransferred;
            if (transferred == 0 || AsyncEvent.SocketError != SocketError.Success)
            {
                this.Disconnect(AsyncEvent);
                this.Recycle(AsyncEvent);
            }
            else
            {
                Token Token = (Token)AsyncEvent.UserToken;

                var buffer = AsyncEvent.Buffer;
                var offset = AsyncEvent.Offset;
                for (int i = 0; i < AsyncEvent.BytesTransferred; i++)
                    Token.Packet.Add(buffer[offset + i]);

                try
                {
                    Token.Process();
                }
                catch (Exception ex)
                {
                    Resources.Exceptions.Catch(ex, "Exception while processing receive");
                }


                if (startNew)
                    this.StartReceive(AsyncEvent);
            }
        }

        internal void Disconnect(SocketAsyncEventArgs AsyncEvent)
        {
            Token Token = (Token) AsyncEvent.UserToken;
            
            if (Token.Device.Player != null)
            {
                if (Resources.Players.ContainsKey(Token.Device.Player.Avatar.UserId))
                {
                    Resources.Players.Remove(Token.Device.Player);
                }
            }
            else
            {
                Resources.Devices.Remove(Token.Device.SocketHandle);
            }
            Interlocked.Decrement(ref ConnectedSockets);
        }

        internal void Send(Message Message)
        {
            var Event = GetArgs();

            var buffer = default(byte[]);
            try
            {
                buffer = Message.ToBytes;
            }
            catch (Exception ex)
            {
                Resources.Exceptions.Catch(ex, $"Exception while constructing message {Message.GetType()}");
                return;
            }

            Event.SetBuffer(buffer, 0, buffer.Length);
            Event.UserToken = Message.Device.Token;

            this.StartSend(Event);
        }

        internal void StartSend(SocketAsyncEventArgs AsyncEvent)
        {
            var client = (Token)AsyncEvent.UserToken;
            var socket = client.Device.Socket;
            
            if (Thread.VolatileRead(ref client.Device.Dropped) == 1)
            {
                this.Recycle(AsyncEvent);
            }
            else
            {
                try
                {
                    while (true)
                    {
                        if (!socket.SendAsync(AsyncEvent))
                            this.ProcessSend(AsyncEvent);
                        else
                            break;
                    }
                }
                catch (ObjectDisposedException)
                {
                    this.Recycle(AsyncEvent);
                }
                catch (Exception ex)
                {
                    Resources.Exceptions.Catch(ex, "Exception while starting send");
                }
            }
        }

        internal void ProcessSend(SocketAsyncEventArgs Args)
        {
            var transferred = Args.BytesTransferred;
            if (transferred == 0 || Args.SocketError != SocketError.Success)
            {
              this.Disconnect(Args);
              this.Recycle(Args);
            }
            else
            {
                try
                {
                    var count = Args.Count;
                    if (transferred < count)
                    {
                        Args.SetBuffer(transferred, count - transferred);
                        this.StartSend(Args);
                    }
                    else
                    {
                        // We done with sending can recycle EventArgs.
                        this.Recycle(Args);
                    }
                }
                catch (Exception ex)
                {
                    Resources.Exceptions.Catch(ex, "Exception while processing send");
                }
            }
            /*while (true)
            {
                Message.Offset += Args.BytesTransferred;

                if (Message.Length + 7 > Message.Offset)
                {
                    if (Message.Device.Connected())
                    {
                        Args.SetBuffer(Message.Offset, Message.Length + 7 - Message.Offset);

                        if (!Message.Device.Socket.SendAsync(Args))
                        {
                            continue;
                        }
                    }
                }
                break;
            }*/
        }

        internal void OnIOCompleted(object sender, SocketAsyncEventArgs AsyncEvent)
        {
            switch (AsyncEvent.LastOperation)
            {
                case SocketAsyncOperation.Accept:
                    this.ProcessAccept(AsyncEvent, true);
                    break;
                case SocketAsyncOperation.Receive:
                    this.ProcessReceive(AsyncEvent, true);
                    break;
                case SocketAsyncOperation.Send:
                    this.ProcessSend(AsyncEvent);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }

        internal void Recycle(SocketAsyncEventArgs AsyncEvent)
        {
            if (AsyncEvent == null)
                return;

            var buffer = AsyncEvent.Buffer;
            AsyncEvent.UserToken = null;
            AsyncEvent.SetBuffer(null, 0, 0);
            AsyncEvent.AcceptSocket = null;
            
            this.ArgsPool.Push(AsyncEvent);

            this.Recycle(buffer);
        }

        internal void Recycle(byte[] buffer)
        {
            if (buffer?.Length == Constants.ReceiveBuffer)
                this.BufferPool.Push(buffer);
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

        internal SocketAsyncEventArgs GetArgs()
        {
            var args = this.ArgsPool.Pop();
            if (args == null)
            {
                args = new SocketAsyncEventArgs();
                args.Completed += OnIOCompleted;
            }
            return args;
        }

        internal byte[] GetBuffer => this.BufferPool.Pop() ?? new byte[Constants.ReceiveBuffer];
    }
}
