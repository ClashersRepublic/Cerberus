﻿using System;
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
        internal Token Token;
        internal Crypto Keys;
        internal IntPtr SocketHandle;
        internal RC4 RC4;
        internal DateTime LastKeepAlive, NextKeepAlive;
        internal Keep_Alive_OK KeepAlive;
        internal string AndroidID, OpenUDID, Model, OSVersion, MACAddress, AdvertiseID, VendorID, IPAddress;
        internal bool Android, Advertising;
        internal int Dropped, Last_Checksum, Last_Tick, Depth;
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
            this.IPAddress = ((IPEndPoint)so.RemoteEndPoint).Address.ToString();
            this.KeepAlive = new Keep_Alive_OK(this);
            this.LastKeepAlive = DateTime.Now;
            this.NextKeepAlive = this.LastKeepAlive.AddSeconds(30);
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
            this.IPAddress = ((IPEndPoint)so.RemoteEndPoint).Address.ToString();
            this.KeepAlive = new Keep_Alive_OK(this);
            this.LastKeepAlive = DateTime.Now;
            this.NextKeepAlive = this.LastKeepAlive.AddSeconds(30);
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
                                Resources.Exceptions.Catch(Exception,
                                    Exception.Message + Environment.NewLine + Exception.StackTrace +
                                    Environment.NewLine + Exception.Data, this.Model, this.OSVersion,
                                    this.Player.Avatar.Token, Player?.Avatar.UserId ?? 0);
                                Loggers.Log(
                                    Utils.Padding(Exception.GetType().Name, 15) + " : " + Exception.Message + ". [" +
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
                        this.Token.Packet.RemoveRange(0, (int)Length + 7);

                        if (Buffer.Length - 7 - Length >= 7)
                        {
                            this.Process(Reader.ReadBytes(Buffer.Length - 7 - (int)Length));
                        }
                    }

                }
            }
        }
    }
}