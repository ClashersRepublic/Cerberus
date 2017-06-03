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
        internal int LastChecksum;

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
            const int HEADER_LEN = 7;
            if (Buffer.Length >= 5)
            {
                int length = (Buffer[2] << 16) | (Buffer[3] << 8) | Buffer[4];
                ushort type = (ushort)((Buffer[0] << 8) | Buffer[1]);
                if (Buffer.Length - HEADER_LEN >= length)
                {
                    var packet = new byte[length];
                    for (int i = 0; i < packet.Length; i++)
                        packet[i] = Buffer[i + HEADER_LEN];

                    if (MessageFactory.Messages.ContainsKey(type))
                    {
                        var Reader = new Reader(packet);
                        Message _Message =
                            Activator.CreateInstance(MessageFactory.Messages[type], this, Reader) as Message;
                        _Message.Identifier = type;
                        _Message.Length = (ushort)length;
                        _Message.Reader = Reader;

                        try
                        {
                            try
                            {
                                if (Constants.Encryption == Enums.Crypto.RC4)
                                    _Message.DecryptRC4();
                                else
                                    _Message.DecryptSodium();
                            }
                            catch (Exception ex)
                            {
                                Resources.Exceptions.Catch(ex,
                                    $"Unable to decrypt message with ID: {type}" + Environment.NewLine +
                                    ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine +
                                    ex.Data, this.Model, this.OSVersion, this.Player.Token,
                                    Player?.UserId ?? 0);
                            }

#if DEBUG
                            Loggers.Log(
                                Utils.Padding(_Message.Device.Socket.RemoteEndPoint.ToString(), 15) + " --> " +
                                _Message.GetType().Name, true);
                            Loggers.Log(_Message,
                                Utils.Padding(_Message.Device.Socket.RemoteEndPoint.ToString(), 15));
#endif

                            try
                            {
                                _Message.Decode();
                            }
                            catch (Exception ex)
                            {
                                Resources.Exceptions.Catch(ex,
                                    $"Unable to decode message with ID: {type}" + Environment.NewLine + ex.Message +
                                    Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Data, this.Model,
                                    this.OSVersion, this.Player.Token, Player?.UserId ?? 0);
                            }
                            try
                            {
                                _Message.Process();
                            }
                            catch (Exception ex)
                            {
                                Resources.Exceptions.Catch(ex,
                                    $"Unable to process message with ID: {type}" + Environment.NewLine +
                                    ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine +
                                    ex.Data, this.Model, this.OSVersion, this.Player.Token,
                                    Player?.UserId ?? 0);
                            }
                        }
                        catch (Exception Exception)
                        {
                            Resources.Exceptions.Catch(Exception,
                                Exception.Message + Environment.NewLine + Exception.StackTrace +
                                Environment.NewLine + Exception.Data, this.Model, this.OSVersion,
                                this.Player.Token, Player?.UserId ?? 0);
                            Loggers.Log(Utils.Padding(Exception.GetType().Name, 15) + " : " + Exception.Message +
                                        ". [" + (this.Player != null
                                            ? this.Player.UserId + ":" +
                                              GameUtils.GetHashtag(this.Player.UserId)
                                            : "---") + ']' + Environment.NewLine + Exception.StackTrace, true,
                                Defcon.ERROR);
                        }
                    }
                    else
                    {
#if DEBUG
                        Loggers.Log(Utils.Padding(this.GetType().Name, 15) + " : Aborting, we can't handle the following message : ID " + type + ", Length " + length + ".", true, Defcon.WARN);
#endif
                        if (Constants.Encryption == Enums.Crypto.RC4)
                        {
                            this.RC4.Decrypt(ref packet);
                        }
                        else
                            this.Crypto.SNonce.Increment();
                    }
                    this.Token.Packet.RemoveRange(0, length + HEADER_LEN);
                }
            }
        }
    }
}