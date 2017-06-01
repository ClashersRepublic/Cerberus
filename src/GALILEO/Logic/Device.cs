using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Core;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Packets;
using BL.Servers.CoC.Packets.Cryptography;

namespace BL.Servers.CoC.Logic
{
    internal class Device
    {
        internal Socket Socket;
        internal Level Player;
        internal Token Token;
        internal Crypto Keys;
        internal RC4Core RC4;
        internal IntPtr SocketHandle;
        internal string AndroidID, OpenUDID, Model, OSVersion, MACAddress, AdvertiseID, VendorID, IPAddress;
        internal bool Android, Advertising;
        internal volatile int Dropped;
        internal int Last_Checksum;
        internal int Last_Tick;
        internal int Depth;
        internal uint ClientSeed;



        public Device(Socket so)
        {
            this.Socket = so;
            this.Keys = new Crypto();
            if (Constants.RC4) 
                this.RC4 = new RC4Core();
            this.SocketHandle = so.Handle;
        }

        public Device(Socket so, Token token)
        {
            this.Socket = so;
            this.Keys = new Crypto();
            if (Constants.RC4)
                this.RC4 = new RC4Core();
            this.Token = token;
            this.SocketHandle = so.Handle;
        }

        internal State State = State.DISCONNECTED;

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

                    if (MessageFactory.Messages.ContainsKey(_Header[0]))
                    {
                        Message _Message =
                            Activator.CreateInstance(MessageFactory.Messages[_Header[0]], this, Reader) as Message;
                        _Message.Identifier = (ushort) _Header[0];
                        _Message.Length = (ushort) _Header[1];
                        _Message.Version = (ushort) _Header[2];

                        _Message.Reader = Reader;

                        try
                        {

                            if (Constants.RC4)
                                _Message.DecryptRC4();
                            else
                                _Message.DecryptPepper();

#if DEBUG
                            Loggers.Log(
                                Utils.Padding(_Message.Device.Socket.RemoteEndPoint.ToString(), 15) + " --> " +
                                _Message.GetType().Name, true);
                            Loggers.Log(_Message, Utils.Padding(_Message.Device.Socket.RemoteEndPoint.ToString(), 15));
#endif
                            _Message.Decode();
                            _Message.Process();
                        }
                        catch (Exception Exception)
                        { 
                            Resources.Exceptions.Catch(Exception, Exception.Message + Environment.NewLine + Exception.StackTrace + Environment.NewLine + Exception.Data, this.Model, this.OSVersion, this.Player.Avatar.Token, Player?.Avatar.UserId ?? 0);
                            Loggers.Log(
                                Utils.Padding(Exception.GetType().Name, 15) + " : " + Exception.Message + ". [" +
                                (this.Player != null
                                    ? this.Player.Avatar.UserId + ":" + GameUtils.GetHashtag(this.Player.Avatar.UserId)
                                    : "---") + ']' + Environment.NewLine + Exception.StackTrace, true, Defcon.ERROR);
                        }
                    }
                    else
                    {
#if DEBUG
                        Loggers.Log(
                            Utils.Padding(this.GetType().Name, 15) +
                            " : Aborting, we can't handle the following message : ID " + _Header[0] + ", Length " +
                            _Header[1] + ", Version " + _Header[2] + ".", true, Defcon.WARN);
#endif
                        if (Constants.RC4)
                        {
                            var Data = Reader.ReadFully();
                            this.RC4.Decrypt(ref Data);
                        }
                        else
                            this.Keys.SNonce.Increment();
                    }
                    this.Token.Packet.RemoveRange(0, _Header[1] + 7);

                    if ((Buffer.Length - 7) - _Header[1] >= 7)
                    {
                        this.Process(Reader.ReadBytes((Buffer.Length - 7) - _Header[1]));
                    }
                }
            }
        }
    }
}