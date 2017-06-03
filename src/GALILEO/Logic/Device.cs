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
using BL.Servers.CoC.Packets.Cryptography.RC4;

namespace BL.Servers.CoC.Logic
{
    internal partial class Device
    {
        internal Socket Socket;
        internal Level Player;
        internal Token Token;
        internal Crypto Keys;
        internal IntPtr SocketHandle;
        internal RC4 RC4;
        internal DateTime LastKeepAlive, NextKeepAlive;
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
            {
                this.RC4 = new RC4();
            }
            this.SocketHandle = so.Handle;
        }

        public Device(Socket so, Token token)
        {
            this.Socket = so;
            this.Keys = new Crypto();
            if (Constants.RC4)
            {
                this.RC4 = new RC4();
            }
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
            const int HEADER_LEN = 7;
            if (Buffer.Length >= 5)
            {
                int length = (Buffer[2] << 16) | (Buffer[3] << 8) | Buffer[4];
                ushort type = (ushort) ((Buffer[0] << 8) | Buffer[1]);
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
                        _Message.Length = (ushort) length;
                        _Message.Reader = Reader;

                        try
                        {
                            try
                            {
                                if (Constants.RC4)
                                    _Message.DecryptRC4();
                                else
                                    _Message.DecryptPepper();
                            }
                            catch (Exception ex)
                            {
                                Resources.Exceptions.Catch(ex,
                                    $"Unable to decrypt message with ID: {type}" + Environment.NewLine +
                                    ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine +
                                    ex.Data, this.Model, this.OSVersion, this.Player.Avatar.Token,
                                    Player?.Avatar.UserId ?? 0);
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
                                    this.OSVersion, this.Player.Avatar.Token, Player?.Avatar.UserId ?? 0);
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
                                    ex.Data, this.Model, this.OSVersion, this.Player.Avatar.Token,
                                    Player?.Avatar.UserId ?? 0);
                            }
                        }
                        catch (Exception Exception)
                        {
                            Resources.Exceptions.Catch(Exception,
                                Exception.Message + Environment.NewLine + Exception.StackTrace +
                                Environment.NewLine + Exception.Data, this.Model, this.OSVersion,
                                this.Player.Avatar.Token, Player?.Avatar.UserId ?? 0);
                            Loggers.Log(Utils.Padding(Exception.GetType().Name, 15) + " : " + Exception.Message +
                                        ". [" + (this.Player != null
                                            ? this.Player.Avatar.UserId + ":" +
                                              GameUtils.GetHashtag(this.Player.Avatar.UserId)
                                            : "---") + ']' + Environment.NewLine + Exception.StackTrace, true,
                                Defcon.ERROR);
                        }
                    }
                    else
                    {
#if DEBUG
                            Loggers.Log(Utils.Padding(this.GetType().Name, 15) + " : Aborting, we can't handle the following message : ID " + type + ", Length " + length + ".", true, Defcon.WARN);
#endif
                        if (Constants.RC4)
                        {
                            this.RC4.Decrypt(ref packet);
                        }
                        else
                            this.Keys.SNonce.Increment();
                    }
                    this.Token.Packet.RemoveRange(0, length + HEADER_LEN);
                }
            }
        }
    }
}