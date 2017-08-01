using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Core.Crypto;

using Magic.ClashOfClans.Core.Settings;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Network;
using Magic.ClashOfClans.Network.Messages.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace Magic.ClashOfClans
{
    internal partial class Client
    {
        internal readonly KeepAliveOkMessage _keepAliveOk;

        public Client(Socket socket)
        {
            _socket = socket;
            _socketHandle = socket.Handle.ToInt64();
            _dataStream = new List<byte>();
            _keepAliveOk = new KeepAliveOkMessage(this);
            _dataStream = new List<byte>(Constants.BufferSize);

            State = ClientState.Exception;

            IncomingPacketsKey = new byte[Key._RC4_EndecryptKey.Length];
            Array.Copy(Key._RC4_EndecryptKey, IncomingPacketsKey, Key._RC4_EndecryptKey.Length);

            OutgoingPacketsKey = new byte[Key._RC4_EndecryptKey.Length];
            Array.Copy(Key._RC4_EndecryptKey, OutgoingPacketsKey, Key._RC4_EndecryptKey.Length);

            LastKeepAlive = DateTime.Now;
            NextKeepAlive = LastKeepAlive.AddSeconds(30);
        }

        private readonly List<byte> _dataStream;
        private readonly Socket _socket;
        private readonly long _socketHandle;

        public ClientState State { get; set; }

        public List<byte> DataStream => _dataStream;
        public Socket Socket => _socket;

        public Level Level { get; set; }

        public long GetSocketHandle() => _socketHandle;
        public uint ClientSeed { get; set; }

        public byte[] IncomingPacketsKey { get; set; }
        public byte[] OutgoingPacketsKey { get; set; }

        public enum ClientState : int
        {
            Exception = 0,
            Login = 1,
            LoginSuccess = 2,
        }

        public DateTime NextKeepAlive { get; set; }
        public DateTime LastKeepAlive { get; set; }

        public bool TryGetPacket(out Message message)
        {
            const int HEADER_LEN = 7;
            message = default(Message);

            var result = false;
            if (DataStream.Count >= 5)
            {
                // Get packet length and type but ignore packet version.
                int length = (DataStream[2] << 16) | (DataStream[3] << 8) | DataStream[4];
                ushort type = (ushort)((DataStream[0] << 8) | DataStream[1]);

                if (DataStream.Count - HEADER_LEN >= length)
                {
                    // Avoid LINQ cause we can.
                    var packet = new byte[length];
                    for (int i = 0; i < packet.Length; i++)
                        packet[i] = DataStream[i + HEADER_LEN];

                    message = MessageFactory.Read(this, new PacketReader(packet), type);
                    if (message != null)
                    {
                        message.Data = packet;
                        message.MessageType = type; // Just in case they don't do it in the constructor.

                        try { message.Decrypt(); }
                        catch (Exception ex)
                        {
                            ExceptionLogger.Log(ex, $"Unable to decrypt message with ID: {type}");
                        }

                        try { message.Decode(); }
                        catch (Exception ex)
                        {
                            ExceptionLogger.Log(ex, $"Unable to decode message with ID: {type}");
                        }
                        result = true;
                    }
                    else
                    {
                        Logger.Say("Unhandled message " + type);

                        // Make sure we don't break the RC4 stream.
                        if (Constants.IsRc4)
                            Decrypt(packet);
                        else
                            CSNonce.Increment();
                    }

                    // Clean up. 
                    DataStream.RemoveRange(0, HEADER_LEN + length);
                }
            }
            return result;
        }
    }
}
