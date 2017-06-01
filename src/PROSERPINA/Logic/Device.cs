using System;
using System.Net.Sockets;
using BL.Servers.CR.Core;
using BL.Servers.CR.Core.Network;
using BL.Servers.CR.Extensions;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic.Enums;
using BL.Servers.CR.Packets;
using BL.Servers.CR.Packets.Cryptography.RC4;
using System.Diagnostics;
using RollbarDotNet;

namespace BL.Servers.CR.Logic
{
    internal class Device
    {
        internal Socket Socket;
        internal Player Player;
        internal Token Token;
        internal Packets.Crypto Crypto;
        internal RC4_Core RC4;

        public Device(Socket so)
        {
            this.Socket = so;
            this.Crypto = new Packets.Crypto();
            this.RC4 = new RC4_Core();
            this.SocketHandle = so.Handle;
        }
        public Device(Socket so, Token token)
        {
            this.Socket = so;
            this.Crypto = new Packets.Crypto();
            this.RC4 = new RC4_Core();
            this.Token = token;
            this.SocketHandle = so.Handle;
        }

        internal State PlayerState = Logic.Enums.State.DISCONNECTED;

        internal IntPtr SocketHandle;

        internal bool Android;

        internal int Ping;
        internal int Tick;
        internal int Major;
        internal int Revision;
        internal int Minor;

        internal volatile int Dropped;

        internal string Interface;
        internal string AndroidID;
        internal string OpenUDID;
        internal string Model;
        internal string OSVersion;
        internal string MACAddress;
        internal string AdvertiseID;
        internal string VendorID;
        internal string IPAddress;

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
                using (Reader Reader = new Reader(Buffer))
                {
                    ushort Identifier = Reader.ReadUInt16();
                    Reader.Seek(1);
                    ushort Length = Reader.ReadUInt16();
                    ushort Version = Reader.ReadUInt16();

                    if (Buffer.Length - 7 >= Length)
                    {
                        if (MessageFactory.Messages.ContainsKey(Identifier))
                        {
                            Packets.Message _Message = Activator.CreateInstance(MessageFactory.Messages[Identifier], this, Reader) as Packets.Message;

                            _Message.Identifier = Identifier;
                            _Message.Length = Length;
                            _Message.Version = Version;

                            _Message.Reader = Reader;

                            try
                            {

                                Debug.WriteLine("[MESSAGE] " + _Message.Device.Socket.RemoteEndPoint.ToString() + " --> " + _Message.GetType().Name + " [" + _Message.Identifier + "]");

                                if (Constants.Encryption == Enums.Crypto.RC4)
                                    _Message.DecryptRC4();
                                else
                                    _Message.DecryptSodium();


                                _Message.Decode();
                                _Message.Process();
                            }
                            catch (System.Exception Exception)
                            {
                                Resources.Exceptions.Catch(Exception, ErrorLevel.Error);
                            }
                        }
                        else
                        {
                            var Data = Reader.ReadFully();

                            Debug.WriteLine("[MESSAGE] Message not found, ignoring the following message : ID " + Identifier + ", Length " + Length + ", Version " + Version + ".");

                            if (Constants.Encryption == Enums.Crypto.RC4)
                            {
                                this.RC4.Decrypt(ref Data);
                                Debug.WriteLine(Identifier + " Data: " + BitConverter.ToString(Data));
                            }
                            else
                                this.Crypto.SNonce.Increment();
                        }

                        this.Token.Packet.RemoveRange(0, Length + 7);

                        if ((Buffer.Length - 7) - Length >= 7)
                        {
                            this.Process(Reader.ReadBytes((Buffer.Length - 7) - Length));
                        }
                    }
                }
            }
        }
    }
}