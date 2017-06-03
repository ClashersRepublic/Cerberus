using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using BL.Servers.CoC.Packets;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using SharpRaven.Data;

namespace BL.Servers.CoC.Core.Networking
{
    internal class Gateway
    {

        internal SocketAsyncEventArgsPool AcceptPool;
        internal SocketAsyncEventArgsPool ReadPool;
        internal SocketAsyncEventArgsPool WritePool;
        internal Pool<byte[]> BufferPool;
        internal Socket Listener;
        //internal Mutex Mutex;
        internal int ConnectedSockets;

        internal Gateway()
        {
            this.AcceptPool = new SocketAsyncEventArgsPool();
            this.ReadPool = new SocketAsyncEventArgsPool();
            this.WritePool = new SocketAsyncEventArgsPool();
            this.BufferPool = new Pool<byte[]>();

            this.Initialize();

            this.Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                ReceiveBufferSize = Constants.ReceiveBuffer,
                SendBufferSize = Constants.SendBuffer,
                Blocking = false,
                NoDelay = true
            };

            this.Listener.Bind(new IPEndPoint(IPAddress.Any, 9339));
            this.Listener.Listen(200);

            Program.Stopwatch.Stop();

            Loggers.Log(
                Assembly.GetExecutingAssembly().GetName().Name +
                $" has been started on {Utils.LocalNetworkIP} in {Math.Round(Program.Stopwatch.Elapsed.TotalSeconds, 4)} Seconds!",
                true);

            this.StartAccept();
        }

        internal void Initialize()
        {
            for (int Index = 0; Index < Constants.PRE_ALLOC_SEA; Index++)
            {
                SocketAsyncEventArgs ReadEvent = new SocketAsyncEventArgs();
                ReadEvent.Completed += this.OnIOCompleted;
                this.ReadPool.Enqueue(ReadEvent);

                SocketAsyncEventArgs WriterEvent = new SocketAsyncEventArgs();
                WriterEvent.Completed += this.OnIOCompleted;
                this.WritePool.Enqueue(WriterEvent);
            }
            for (int Index = 0; Index < 5; Index++)
            {
                SocketAsyncEventArgs AcceptEvent = new SocketAsyncEventArgs();
                AcceptEvent.Completed += this.OnIOCompleted;
                this.AcceptPool.Enqueue(AcceptEvent);
            }
        }

        internal void StartAccept()
        {
            SocketAsyncEventArgs AcceptEvent = this.AcceptPool.Dequeue();
            if (AcceptEvent == null)
            {
                AcceptEvent = new SocketAsyncEventArgs();
                AcceptEvent.Completed += this.OnIOCompleted;
            }
            while (true)
            {
                if (!this.Listener.AcceptAsync(AcceptEvent))
                    this.ProcessAccept(AcceptEvent, false);
                else
                    break;
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
                    //ExceptionLogger.Log(ex, "Exception while start receive: ");
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
                        this.AcceptPool.Enqueue(AsyncEvent);
                        if (startNew)
                            StartAccept();
                        return;
                    }
                }


                Loggers.Log($"New client connected -> {((IPEndPoint)Socket.RemoteEndPoint).Address}", true);

                SocketAsyncEventArgs ReadEvent = this.ReadPool.Dequeue();

                if (ReadEvent != null)
                {
                    var buffer = GetBuffer;
                    ReadEvent.AcceptSocket = Socket;
                    ReadEvent.SetBuffer(buffer, 0, buffer.Length);
                    Device device = new Device(Socket)
                    {
                        IPAddress = ((IPEndPoint)Socket.RemoteEndPoint).Address.ToString()
                    };

                    Token Token = new Token(ReadEvent, device);
                    device.Token = Token;
                    Interlocked.Increment(ref this.ConnectedSockets);
                    Resources.Devices.Add(device);

                    this.StartReceive(ReadEvent);

                }
                else
                {
                    try
                    {

                        ReadEvent = new SocketAsyncEventArgs();
                        ReadEvent.SetBuffer(new byte[Constants.ReceiveBuffer], 0, Constants.ReceiveBuffer);
                        ReadEvent.Completed += this.OnIOCompleted;
                        ReadEvent.AcceptSocket = Socket;

                        Device device = new Device(Socket)
                        {
                            IPAddress = ((IPEndPoint)Socket.RemoteEndPoint).Address.ToString()
                        };

                        Token Token = new Token(ReadEvent, device);
                        device.Token = Token;
                        Interlocked.Increment(ref this.ConnectedSockets);
                        Resources.Devices.Add(device);

                        this.StartReceive(ReadEvent);
                    }
                    catch (Exception ex)
                    {
                        Resources.Exceptions.RavenClient.Capture(
                            new SentryEvent("There are no more available sockets to allocate."));
                    }
                }
            }
            else
            {
                Loggers.Log("Not connected or error at ProcessAccept.", false, Defcon.ERROR);
                Socket.Close(5);
            }

            AsyncEvent.AcceptSocket = null;
            this.AcceptPool.Enqueue(AsyncEvent);
            if (startNew)
                StartAccept();
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
                Token Token = AsyncEvent.UserToken as Token;

                Token.SetData();

                try
                {
                    if (Token.Device.Socket.Available == 0)
                    {
                        Token.Process();
                    }
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
            Token Token = AsyncEvent.UserToken as Token;

            Token.Aborting = true;

            if (Token.Device.Player != null)
            {
                if (Resources.Players.ContainsValue(Token.Device.Player))
                {
                    Resources.Players.Remove(Token.Device.Player);
                }
            }
            else if (!Token.Device.Connected())
            {
                Resources.Devices.Remove(Token.Device);
            }

            Interlocked.Decrement(ref ConnectedSockets);
        }

        internal void Send(Message Message)
        {
            SocketAsyncEventArgs WriteEvent = this.WritePool.Dequeue();

            if (WriteEvent != null)
            {
                WriteEvent.SetBuffer(Message.ToBytes, Message.Offset, Message.Length + 7 - Message.Offset);

                WriteEvent.AcceptSocket = Message.Device.Socket;
                WriteEvent.RemoteEndPoint = Message.Device.Socket.RemoteEndPoint;
                WriteEvent.UserToken = Message.Device.Token;

                this.StartSend(WriteEvent);
            }
            else
            {
                WriteEvent = new SocketAsyncEventArgs();

                WriteEvent.SetBuffer(Message.ToBytes, Message.Offset, Message.Length + 7 - Message.Offset);

                WriteEvent.Completed += this.OnIOCompleted;

                WriteEvent.AcceptSocket = Message.Device.Socket;
                WriteEvent.RemoteEndPoint = Message.Device.Socket.RemoteEndPoint;
                WriteEvent.UserToken = Message.Device.Token;

                this.StartSend(WriteEvent);
            }
        }

        internal void StartSend(SocketAsyncEventArgs AsyncEvent)
        {
            var client = (Token)AsyncEvent.UserToken;
            var socket = client.Device.Socket;

            if (Thread.VolatileRead(ref client.Device.Dropped) == 1)
            {
                this.Recycle(AsyncEvent, false);
            }
            else
            {
                try
                {
                    while (true)
                    {
                        if (!socket.SendAsync(AsyncEvent))
                            this.ProcessSend(AsyncEvent);
                        else break;
                    }
                }
                catch (ObjectDisposedException)
                {
                    this.Recycle(AsyncEvent, false);
                }
                catch (Exception ex)
                {
                    Resources.Exceptions.Catch(ex, "Exception while starting receive");
                }
            }
        }

        internal void ProcessSend(SocketAsyncEventArgs Args)
        {
            var client = (Token)Args.UserToken;
            var transferred = Args.BytesTransferred;
            if (transferred == 0 || Args.SocketError != SocketError.Success)
            {
              this.Disconnect(Args);
              this.Recycle(Args, false);
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
                        this.Recycle(Args, false);
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

        internal void Recycle(SocketAsyncEventArgs AsyncEvent, bool read = true)
        {
            if (AsyncEvent == null)
                return;

            var buffer = AsyncEvent.Buffer;
            AsyncEvent.UserToken = null;
            AsyncEvent.SetBuffer(null, 0, 0);
            AsyncEvent.AcceptSocket = null;

            if (read)
                this.ReadPool.Enqueue(AsyncEvent);
            else

                this.WritePool.Enqueue(AsyncEvent);
            this.Recycle(buffer);
        }

        internal void Recycle(byte[] buffer)
        {
            if (buffer?.Length == Constants.ReceiveBuffer)
                this.BufferPool.Push(buffer);
        }

        internal byte[] GetBuffer => this.BufferPool.Pop() ?? new byte[Constants.ReceiveBuffer];
    }
}
