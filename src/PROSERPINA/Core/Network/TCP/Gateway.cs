using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using BL.Servers.CR.Packets;
using System.Linq;
using System.Reflection;
using System.Text;
using BL.Servers.CR.Logic;
using BL.Servers.CR.Logic.Enums;
using BL.Servers.CR.Packets.Messages.Server;

namespace BL.Servers.CR.Core.Network.TCP
{
    internal class Gateway
    {

        internal SocketAsyncEventArgsPool ReadPool;
        internal SocketAsyncEventArgsPool WritePool;
        internal Socket Listener;
        internal Mutex Mutex;

        internal int ConnectedSockets;

        internal Gateway()
        {
            this.ReadPool = new SocketAsyncEventArgsPool();
            this.WritePool = new SocketAsyncEventArgsPool();

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

            Loggers.Log(Assembly.GetExecutingAssembly().GetName().Name + $" has been started on {this.Listener.LocalEndPoint} in {Math.Round(Program.Stopwatch.Elapsed.TotalSeconds, 4)} Seconds!", true);

            SocketAsyncEventArgs AcceptEvent = new SocketAsyncEventArgs();
            AcceptEvent.Completed += this.OnAcceptCompleted;

            this.StartAccept(AcceptEvent);
        }

        internal void Initialize()
        {
            for (int Index = 0; Index < Constants.MaxPlayers + 100; Index++)
            {
                SocketAsyncEventArgs ReadEvent = new SocketAsyncEventArgs();
                ReadEvent.SetBuffer(new byte[Constants.ReceiveBuffer], 0, Constants.ReceiveBuffer);
                ReadEvent.Completed += this.OnReceiveCompleted;
                this.ReadPool.Enqueue(ReadEvent);

                SocketAsyncEventArgs WriterEvent = new SocketAsyncEventArgs();
                WriterEvent.Completed += this.OnSendCompleted;
                this.WritePool.Enqueue(WriterEvent);
            }
        }

        internal void StartAccept(SocketAsyncEventArgs AcceptEvent)
        {
            AcceptEvent.AcceptSocket = null;

            if (!this.Listener.AcceptAsync(AcceptEvent))
            {
                this.ProcessAccept(AcceptEvent);
            }
        }

        internal void ProcessAccept(SocketAsyncEventArgs AsyncEvent)
        {
            Socket Socket = AsyncEvent.AcceptSocket;

            if (Socket.Connected && AsyncEvent.SocketError == SocketError.Success)
            {
                if (Constants.Local)
                {
                    if (!Constants.AuthorizedIP.Contains(Socket.RemoteEndPoint.ToString().Split(':')[0]))
                    {
                        Socket.Close();

                        AsyncEvent.AcceptSocket = null;
                        this.StartAccept(AsyncEvent);
                        return;
                    }
                }

                Loggers.Log($"New client connected -> {((IPEndPoint)Socket.RemoteEndPoint).Address}", true);

                SocketAsyncEventArgs ReadEvent = this.ReadPool.Dequeue();

                if (ReadEvent != null)
                {
                    Device device = new Device(Socket)
                    {
                        IPAddress = ((IPEndPoint) Socket.RemoteEndPoint).Address.ToString()
                    };

                    Token Token = new Token(ReadEvent, device);
                    device.Token = Token;
                    Interlocked.Increment(ref this.ConnectedSockets);
                    Resources.Devices.Add(device);

                    try
                    {
                        if (!Socket.ReceiveAsync(ReadEvent))
                        {
                            this.ProcessReceive(ReadEvent);
                        }
                    }
                    catch (Exception)
                    {
                        this.Disconnect(ReadEvent);
                    }
                }
            }
            else
            {
                Loggers.Log("Not connected or error at ProcessAccept.",false, Defcon.ERROR);
                Socket.Close(5);
            }

            this.StartAccept(AsyncEvent);
        }

        internal void ProcessReceive(SocketAsyncEventArgs AsyncEvent)
        {
            if (AsyncEvent.BytesTransferred > 0 && AsyncEvent.SocketError == SocketError.Success)
            {
                Token Token = AsyncEvent.UserToken as Token;

                Token.SetData();

                try
                {
                    if (Token.Device.Socket.Available == 0)
                    {
                        Token.Process();

                        if (!Token.Aborting)
                        {
                            if (!Token.Device.Socket.ReceiveAsync(AsyncEvent))
                            {
                                this.ProcessReceive(AsyncEvent);
                            }
                        }
                    }
                    else
                    {
                        if (!Token.Aborting)
                        {
                            if (!Token.Device.Socket.ReceiveAsync(AsyncEvent))
                            {
                                this.ProcessReceive(AsyncEvent);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    this.Disconnect(AsyncEvent);
                }
            }
            else
            {
                this.Disconnect(AsyncEvent);
            }
        }

        internal void OnReceiveCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            this.ProcessReceive(AsyncEvent);
        }

        internal void Disconnect(SocketAsyncEventArgs AsyncEvent)
        {
            Token Token = AsyncEvent.UserToken as Token;

            Token.Aborting = true;

            if (Token.Device.Player != null)
            {
                if (Resources.Players.ContainsValue(Token.Device.Player))
                {
                    if (Token.Device.PlayerState == State.IN_BATTLE)
                    {
                        Token.Device.Socket.Shutdown(SocketShutdown.Both);

                        Resources.Players.Remove(Token.Device.Player);
                    }
                    else
                    {
                        Resources.Players.Remove(Token.Device.Player);
                    }
                }
            }
            else if (!Token.Device.Connected())
            {
                Resources.Devices.Remove(Token.Device);
            }

            this.ReadPool.Enqueue(AsyncEvent);
        }

        internal void OnAcceptCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            this.ProcessAccept(AsyncEvent);
        }

        internal void Send(Message Message)
        {
            SocketAsyncEventArgs WriteEvent = this.WritePool.Dequeue();

            if (WriteEvent != null)
            {
                Console.WriteLine("WriteEvent is not null!");

                WriteEvent.SetBuffer(Message.ToBytes, Message.Offset, Message.Length + 7 - Message.Offset);

                WriteEvent.AcceptSocket = Message.Device.Socket;
                WriteEvent.RemoteEndPoint = Message.Device.Socket.RemoteEndPoint;

                if (!Message.Device.Socket.SendAsync(WriteEvent))
                {
                    this.ProcessSend(Message, WriteEvent);
                }
            }
            else
            {
                Console.WriteLine("WriteEvent is null!");

                WriteEvent = new SocketAsyncEventArgs();

                WriteEvent.SetBuffer(Message.ToBytes, Message.Offset, Message.Length + 7 - Message.Offset);

                WriteEvent.AcceptSocket = Message.Device.Socket;
                WriteEvent.RemoteEndPoint = Message.Device.Socket.RemoteEndPoint;

                if (!Message.Device.Socket.SendAsync(WriteEvent))
                {
                    this.ProcessSend(Message, WriteEvent);
                }
            }
        }

        internal void ProcessSend(Message Message, SocketAsyncEventArgs Args)
        {
            Message.Offset += Args.BytesTransferred;

            if (Message.Length + 7 > Message.Offset)
            {
                if (Message.Device.Connected())
                {
                    Args.SetBuffer(Message.Offset, Message.Length + 7 - Message.Offset);

                    if (!Message.Device.Socket.SendAsync(Args))
                    {
                        this.ProcessSend(Message, Args);
                        Console.WriteLine("Process send complete!");
                    }
                }
            }
        }

        internal void OnSendCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            this.WritePool.Enqueue(AsyncEvent);
        }
    }
}
