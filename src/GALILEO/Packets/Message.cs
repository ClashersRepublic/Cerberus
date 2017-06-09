using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.External.Sodium;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Packets.Messages.Server;

namespace BL.Servers.CoC.Packets
{
    internal class Message
    {
        internal ushort Identifier;
        internal ushort Length;
        internal ushort Version;

        internal Device Device;
        internal Reader Reader;
        internal List<byte> Data;

        internal int Offset;
        internal Message(Device Device)
        {
            this.Device = Device;
            this.Data = new List<byte>();
        }

        internal Message(Device Device, Reader Reader)
        {
            this.Device = Device;
            this.Reader = Reader;
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> Packet = new List<byte>();

                Packet.AddUShort(this.Identifier);
                Packet.Add(0);
                Packet.AddUShort(this.Length);
                Packet.AddUShort(this.Version);
                Packet.AddRange(this.Data);

                return Packet.ToArray();
            }
        }

        internal virtual void Decode()
        {

        }

        internal virtual void Encode()
        {

        }

        internal virtual void Process()
        {
        }

        internal virtual void DecryptPepper()
        {
            if (this.Device.State >= State.LOGGED)
            {
                this.Device.Keys.SNonce.Increment();

                byte[] Decrypted = Sodium.Decrypt(new byte[16].Concat(this.Reader.ReadBytes(this.Length)).ToArray(),
                    this.Device.Keys.SNonce, this.Device.Keys.PublicKey);

                if (Decrypted == null)
                {
                    throw new CryptographicException("Tried to decrypt an incomplete message.");
                }

                this.Reader = new Reader(Decrypted);
                this.Length = (ushort) this.Reader.BaseStream.Length;
            }
        }

        internal virtual void DecryptRC4()
        {
            byte[] Decrypted = this.Reader.ReadBytes(this.Length).ToArray();
            if (this.Identifier != 10100)
            {
                //Console.WriteLine($"Raw {BitConverter.ToString(Decrypted.ToArray()).Replace("-", "")}");
                //Console.WriteLine($"Buffer Lenght {Decrypted.ToArray().Length}");
                this.Device.RC4.Decrypt(ref Decrypted);
            }
            this.Reader = new Reader(Decrypted);
            this.Length = (ushort) this.Reader.BaseStream.Length;
            //Console.WriteLine($"Decrypted {BitConverter.ToString(Decrypted.ToArray()).Replace("-", "")}");
            //Console.WriteLine(this.Length);
        }

        internal virtual void EncryptPepper()
        {
            if (this.Device.State >= State.LOGGED)
            {
                this.Device.Keys.RNonce.Increment();

                this.Data = new List<byte>(Sodium.Encrypt(this.Data.ToArray(), this.Device.Keys.RNonce,
                        this.Device.Keys.PublicKey)
                    .Skip(16)
                    .ToArray());
            }

            this.Length = (ushort) this.Data.Count;
        }

        internal virtual void EncryptRC4()
        {
            byte[] Encrypted = this.Data.ToArray();

            if (this.Device.State > State.SESSION_OK)
                this.Device.RC4.Encrypt(ref Encrypted);

            this.Data = new List<byte>(Encrypted);
            this.Length = (ushort) this.Data.Count;
        }

        internal void Debug()
        {
            Console.WriteLine(this.GetType().Name + " : " +
                              BitConverter.ToString(
                                  this.Reader.ReadBytes(
                                      (int) (this.Reader.BaseStream.Length - this.Reader.BaseStream.Position))));
        }

        internal void SendChatMessage(string message)
        {
            new Global_Chat_Entry(this.Device)
            {
                Message = message,
                Message_Sender = this.Device.Player.Avatar,
                Bot = true
            }.Send();
        }

        internal void ShowValues()
        {
            Console.WriteLine(Environment.NewLine);

            foreach (FieldInfo Field in this.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (Field != null)
                {
                    Console.WriteLine(Utils.Padding(this.GetType().Name) + " - " + Utils.Padding(Field.Name) + " : " +
                                      Utils.Padding(
                                          !string.IsNullOrEmpty(Field.Name)
                                              ? (Field.GetValue(this) != null
                                                  ? Field.GetValue(this).ToString()
                                                  : "(null)")
                                              : "(null)", 40));
                }
            }
        }
    }
}