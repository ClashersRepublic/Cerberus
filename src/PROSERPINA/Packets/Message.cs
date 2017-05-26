using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using BL.Servers.CR.Library.Sodium;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic.Enums;
using BL.Servers.CR.Core;
using BL.Servers.CR.Extensions;
using BL.Servers.CR.Extensions.List;
using BL.Servers.CR.Logic;
using BL.Servers.CR.Packets.Messages.Server.Authentication;
using BL.Servers.CR.Core.Network;

namespace BL.Servers.CR.Packets
{
    internal class Message
    {
        internal ushort Identifier;
        internal ushort Length;
        internal ushort Version;

        internal int Offset;

        internal Device Device;

        internal Reader Reader;

        internal List<byte> Data;

        internal Message(Device Device)
        {
            this.Device = Device;
            this.Data = new List<byte>(Constants.SendBuffer);
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

        internal virtual void DecryptSodium()
        {
            if (this.Device.PlayerState >= State.LOGGED)
            {
                this.Device.Crypto.SNonce.Increment();

                byte[] Decrypted = Sodium.Decrypt(new byte[16].Concat(this.Reader.ReadBytes(this.Length)).ToArray(), this.Device.Crypto.SNonce, this.Device.Crypto.PublicKey);

                if (Decrypted == null)
                {
                    throw new CryptographicException("Tried to decrypt an incomplete message.");
                }

                this.Reader = new Reader(Decrypted);
                this.Length = (ushort)this.Reader.BaseStream.Length;
            }
        }

        internal virtual void EncryptSodium()
        {
            if (this.Device.PlayerState >= State.LOGGED)
            {
                this.Device.Crypto.RNonce.Increment();

                this.Data = new List<byte>(Sodium.Encrypt(this.Data.ToArray(), this.Device.Crypto.RNonce, this.Device.Crypto.PublicKey).Skip(16).ToArray());
            }

            this.Length = (ushort)this.Data.Count;
        }

        internal virtual void DecryptRC4()
        {
            byte[] Decrypted = this.Reader.ReadBytes(this.Length);

            if (this.Identifier != 10100)
            {
                this.Device.RC4.Decrypt(ref Decrypted);
            }

            this.Reader = new Reader(Decrypted);
            this.Length = (ushort)this.Reader.BaseStream.Length;
        }

        internal virtual void EncryptRC4()
        {
            byte[] Encrypted = this.Data.ToArray();
            if (this.Device.PlayerState > State.SESSION_OK)
                this.Device.RC4.Encrypt(ref Encrypted);

            this.Data = new List<byte>(Encrypted);
            this.Length = (ushort)this.Data.Count;
        }

        internal void Debug()
        {
            Console.WriteLine(this.GetType().Name + " : " + BitConverter.ToString(this.Reader.ReadBytes((int)(this.Reader.BaseStream.Length - this.Reader.BaseStream.Position))));
        }

        internal void ShowValues()
        {
            Console.WriteLine(Environment.NewLine);

            foreach (FieldInfo Field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (Field != null)
                {
                    Console.WriteLine(Utils.Padding(this.GetType().Name) + " - " + Utils.Padding(Field.Name) + " : " + Utils.Padding(!string.IsNullOrEmpty(Field.Name) ? (Field.GetValue(this) != null ? Field.GetValue(this).ToString() : "(null)") : "(null)", 40));
                }
            }
        }
    }
}
