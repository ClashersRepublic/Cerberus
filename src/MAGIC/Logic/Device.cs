using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using CRepublic.Magic.Core;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Packets;
using CRepublic.Magic.Packets.Cryptography;
using CRepublic.Magic.Packets.Cryptography.RC4;
using CRepublic.Magic.Packets.Messages.Server;
using System.Net;

namespace CRepublic.Magic.Logic
{
    internal class Device
    {
        internal Socket Socket;
        internal Level Player;
        internal Crypto Keys;
        internal IntPtr SocketHandle;
        internal RC4 RC4;
        internal DateTime LastKeepAlive, NextKeepAlive;
        internal Keep_Alive_OK KeepAlive;
        internal string AndroidID, OpenUDID, Model, OSVersion, MACAddress, AdvertiseID, VendorID, IPAddress;
        internal bool Android, Advertising;
        internal int Last_Checksum, Last_Tick, Depth;
        internal uint ClientSeed;
        internal readonly List<byte> Stream;



        public Device(Socket so)
        {
            this.Socket = so;
            this.Keys = new Crypto();
            if (Constants.RC4)
            {
                this.RC4 = new RC4();
            }
            this.SocketHandle = so.Handle;
            this.IPAddress = ((IPEndPoint)so.RemoteEndPoint).Address.ToString();
            this.KeepAlive = new Keep_Alive_OK(this);
            this.LastKeepAlive = DateTime.Now;
            this.NextKeepAlive = this.LastKeepAlive.AddSeconds(30);
            this.Stream = new List<Byte>();
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

        internal bool TryGetPacket(out Message message)
        {
            const int HEADER_LEN = 7;
            message = default(Message);
            var result = false;
            if (Stream.Count >= 5)
            {
                int length = (Stream[2] << 16) | (Stream[3] << 8) | Stream[4];
                ushort type = (ushort) ((Stream[0] << 8) | Stream[1]);

                if (Stream.Count - HEADER_LEN >= length)
                {
                    // Avoid LINQ cause we can.
                    var packet = new byte[length];
                    for (int i = 0; i < packet.Length; i++)
                        packet[i] = Stream[i + HEADER_LEN];

                    message = MessageFactory.Parse(this, new Reader(packet), type);
                    if (message != null)
                    {
                        message.Identifier = type;
                        message.Length = (uint) length;
                        try
                        {
                            //if (Constants.RC4)
                            message.DecryptRC4();
                            //else
                            //message.DecryptPepper();
                        }
                        catch (Exception ex)
                        {
                            //ExceptionLogger.Log(ex, $"Unable to decrypt message with ID: {type}");
                        }

                        try
                        {
                            message.Decode();
                        }
                        catch (Exception ex)
                        {
                            //ExceptionLogger.Log(ex, $"Unable to decode message with ID: {type}");
                        }
#if DEBUG
                        Loggers.Log(
                            Utils.Padding(message.Device.Socket.RemoteEndPoint.ToString(), 15) + " --> " +
                            message.GetType().Name, true);
#endif
                        result = true;
                    }
                    else
                    {
                        //Logger.Say("Unhandled message " + type);

                        // Make sure we don't break the RC4 stream.
                        if (Constants.RC4)
                        {
                            this.RC4.Decrypt(ref packet);
                            packet = null;
                        }
                        else
                            this.Keys.SNonce.Increment();
                    }
                    Stream.RemoveRange(0, HEADER_LEN + length);
                }
            }
            return result;
        }

        [Obsolete]
        internal void Process()
        {
            var Buffer = Stream.ToArray();
            if (Buffer.Length >= 7)
            {
                using (Reader Reader = new Reader(Buffer))
                {
                    ushort Identifier = Reader.ReadUInt16();
                    uint Length = Reader.ReadUInt24();
                    ushort Version = Reader.ReadUInt16();
                    if (Buffer.Length - 7 >= Length)
                    {
                        if (MessageFactory.Messages.ContainsKey(Identifier))
                        {
                            Message _Message = Activator.CreateInstance(MessageFactory.Messages[Identifier], this, Reader) as Message;
                            _Message.Identifier = Identifier;
                            _Message.Length = Length;
                            _Message.Version = Version;
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
#endif
                                _Message.Decode();
                                _Message.Process();
                            }

                            catch (Exception Exception)
                            {
                                Exceptions.Log(Exception,
                                    Exception.Message + Environment.NewLine + Exception.StackTrace +
                                    Environment.NewLine + Exception.Data, this.Model, this.OSVersion,
                                    this.Player.Avatar.Token, Player?.Avatar.UserId ?? 0);
                                Loggers.Log(Utils.Padding(Exception.GetType().Name, 15) + " : " + Exception.Message + ". [" +
                                    (this.Player != null
                                        ? this.Player.Avatar.UserId + ":" +
                                          GameUtils.GetHashtag(this.Player.Avatar.UserId)
                                        : "---") + ']' + Environment.NewLine + Exception.StackTrace, true,
                                    Defcon.ERROR);
                            }
                        }
                        else
                        {
#if DEBUG
                            Loggers.Log(
                                Utils.Padding(this.GetType().Name, 15) +
                                " : Aborting, we can't handle the following message : ID " + Identifier + ", Length " +
                                Length + ".", true, Defcon.WARN);
#endif
                            if (Constants.RC4)
                            {
                                var buffer = Reader.ReadBytes((int)Length);
                                this.RC4.Decrypt(ref buffer);
                                buffer = null;
                            }
                            else
                                this.Keys.SNonce.Increment();
                        }
                        Stream.RemoveRange(0, (int)Length + 7);

                        if (Buffer.Length - 7 - Length >= 7)
                        {
                            this.Process();
                        }
                    }

                }
            }
        }
    }
}