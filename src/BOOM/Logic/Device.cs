using System;
using System.Net.Sockets;
using CRepublic.Boom.Core;
using CRepublic.Boom.Core.Network;
using CRepublic.Boom.Extensions;
using CRepublic.Boom.Extensions.Binary;
using CRepublic.Boom.Logic.Enums;
using CRepublic.Boom.Packets;

namespace CRepublic.Boom.Logic
{
    internal class Device
    {
        internal Socket Socket;
        internal Level Player;
        internal Token Token;
        internal Crypto Keys;

        public Device(Socket so)
        {
            this.Socket = so;
            this.Keys = new Crypto();
            this.SocketHandle = so.Handle;
        }
        public Device(Socket so, Token token)
        {
            this.Socket = so;
            this.Keys = new Crypto();
            this.Token = token;
            this.SocketHandle = so.Handle;
        }


        internal State PlayerState = Logic.Enums.State.DISCONNECTED;

        internal IntPtr SocketHandle;

        internal string Interface;
        internal string AndroidID;
        internal string OpenUDID;
        internal string Model;
        internal string OSVersion;
        internal string MACAddress;
        internal string AdvertiseID;
        internal string VendorID;
        internal string IPAddress;

        internal uint ClientSeed;

        public int Major { get; internal set; }
        public int Minor { get; internal set; }
        public int Revision { get; internal set; }
        public bool Android { get; internal set; }

        public bool Connected()
        {
            try
            {
                return !(Socket.Poll(1000, SelectMode.SelectRead) && Socket.Available == 0 || !Socket.Connected);
            }
            catch
            {
                return false;
            }
        }

        internal void Process(byte[] Buffer)
        {
            if (Buffer.Length >= 7)
            {
                int[] _Header = new int[3];

                using (Reader Reader = new Reader(Buffer))
                {
                    _Header[0] = Reader.ReadUInt16(); // Message ID
                    Reader.Seek(1);
                    _Header[1] = Reader.ReadUInt16(); // Length
                    _Header[2] = Reader.ReadUInt16(); // Version

                    if (Buffer.Length - 7 >= _Header[1])
                    {
                        if (MessageFactory.Messages.ContainsKey(_Header[0]))
                        {
                            Message _Message =  Activator.CreateInstance(MessageFactory.Messages[_Header[0]], this, Reader) as Message;

                            _Message.Identifier = (ushort)_Header[0];
                            _Message.Length = (ushort)_Header[1];
                            _Message.Version = (ushort)_Header[2];

                            _Message.Reader = Reader;

                            try
                            {

#if DEBUG
                                Loggers.Log(Utils.Padding(_Message.Device.Socket.RemoteEndPoint.ToString(), 15) + " --> " + _Message.GetType().Name, true);
#endif
                                _Message.Decrypt();
                                _Message.Decode();
                                _Message.Process();
                            }
                            catch (Exception Exception)
                            {
                                Loggers.Log(Utils.Padding(Exception.GetType().Name, 15) + " : " + Exception.Message + ". [" + (this.Player != null ? this.Player.Avatar.UserHighId + ":" + this.Player.Avatar.UserLowId : "---") + ']' + Environment.NewLine + Exception.StackTrace, true, Defcon.ERROR);
                            }
                        }
                        else
                        {
#if DEBUG

                            Loggers.Log(Utils.Padding(this.GetType().Name, 15) + " : Aborting, we can't handle the following message : ID " + _Header[0] + ", Length " + _Header[1] + ", Version " + _Header[2] + ".", true, Defcon.WARN);

#endif
                            this.Keys.SNonce.Increment();
                        }

                        this.Token.Packet.RemoveRange(0, _Header[1] + 7);

                        if ((Buffer.Length - 7) - _Header[1] >= 7)
                        {
                            this.Process(Reader.ReadBytes((Buffer.Length - 7) - _Header[1]));
                        }
                       // else
                        //{
                         //   this.Token.Reset();
                        //}
                    }
                }
            }
        }
    }
}