using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Magic.Core.Interface;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Packets;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Packets.Messages.Server;

namespace CRepublic.Magic.Logic
{
    internal partial class Device
    {
        internal Socket Socket { get; }
        internal IntPtr SocketHandle { get; }
        internal List<byte> Stream { get; }
        internal State State = State.DISCONNECTED;

        internal uint ClientSeed { get; set; }

        internal byte[] IncomingPacketsKey { get; set; }
        internal byte[] OutgoingPacketsKey { get; set; }

        internal DateTime NextKeepAlive { get; set; }
        internal DateTime LastKeepAlive { get; set; }

        internal readonly Keep_Alive_OK Keep_Alive;


        internal string AndroidID { get; set; }
        internal string OpenUDID { get; set; }
        internal string Model { get; set; }
        internal string OSVersion{ get; set; }
        internal string MACAddress { get; set; }
        internal string AdvertiseID { get; set; }
        internal string VendorID { get; set; }
        internal string IPAddress { get; set; }
        internal bool Android { get; set; }
        internal bool Advertising { get; set; }

        public Device(Socket socket)
        {
            this.Socket = socket;
            this.SocketHandle = socket.Handle;
            this.Stream = new List<byte>(Constants.Buffer); 
            this.Keep_Alive = new Keep_Alive_OK(this);

            this.IncomingPacketsKey = new byte[Key._RC4_EndecryptKey.Length];
            Array.Copy(Key._RC4_EndecryptKey, this.IncomingPacketsKey, Key._RC4_EndecryptKey.Length);

            this.OutgoingPacketsKey = new byte[Key._RC4_EndecryptKey.Length];
            Array.Copy(Key._RC4_EndecryptKey, this.OutgoingPacketsKey, Key._RC4_EndecryptKey.Length);

            this.LastKeepAlive = DateTime.Now;
            this.NextKeepAlive = this.LastKeepAlive.AddSeconds(30);
        }

        internal bool TryGetPacket(out Message message)
        {
            const int HEADER_LEN = 7;
            message = default(Message);
            var result = false;
            if (Stream.Count >= 5)
            {
                int length = (Stream[2] << 16) | (Stream[3] << 8) | Stream[4];
                ushort type = (ushort)((Stream[0] << 8) | Stream[1]);

                if (Stream.Count - HEADER_LEN >= length)
                {
                    // Avoid LINQ cause we can.
                    var packet = new byte[length];
                    for (int i = 0; i < packet.Length; i++)
                    {
                        packet[i] = Stream[i + HEADER_LEN];
                    }

                    message = MessageFactory.Parse(this, type);
                    if (message != null)
                    {
                        message.Data = new List<byte>(packet);
                        message.Identifier = type;
                        try
                        {
                            //if (Constants.RC4)
                            message.Decrypt();
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
                       //Control.Say(message.Device.Socket.RemoteEndPoint + " --> " + message.GetType().Name);
#endif
                        result = true;
                    }
                    else
                    {
                        Console.WriteLine("Unhandled message " + type);
                        //Logger.Say("Unhandled message " + type);

                        // Make sure we don't break the RC4 stream.
                            this.Decrypt(packet.ToArray());
                            packet = null;
                    }
                    Stream.RemoveRange(0, HEADER_LEN + length);
                }
            }
            return result;
        }
    }
}
