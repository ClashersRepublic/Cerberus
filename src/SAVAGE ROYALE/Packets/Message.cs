using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CRepublic.Royale.Core.Interface;
using CRepublic.Royale.Extensions;
using CRepublic.Royale.Extensions.Binary;
using CRepublic.Royale.Extensions.List;
using CRepublic.Royale.External.Sodium;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Logic.Enums;

namespace CRepublic.Royale.Packets
{
    internal class Message
    {
        internal Device Device { get; set; }
        internal ushort Identifier;
        internal ushort Version;
        internal int Offset;
        internal int Length { get; set; }
        internal Reader Reader { get; set; }

        internal List<byte> Data
        {
            get => _data;
            set
            {
                _data = value;
                Length = value.Count;
            }
        }

        private List<byte> _data;

        internal Message(Device Device)
        {
            this.Device = Device;
            this.Data = new List<byte>();
        }

        /*internal Message(Device Device, Reader Reader)
        {
            this.Device = Device;
            this.Reader = Reader;
        }*/

        internal byte[] ToBytes
        {
            get
            {
                List<byte> Packet = new List<byte>();

                Packet.AddUShort(this.Identifier);
                Packet.AddInt24(this.Length);
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

        internal virtual void Decrypt()
        {
            if (this.Device.State >= State.LOGGED)
            {
                this.Device.SNonce.Increment();

                var Decrypted = Sodium.Decrypt(new byte[16].Concat(this.Data).ToArray(), this.Device.SNonce, this.Device.PublicKey);

                this.Reader = new Reader(Decrypted);
                this.Length = (ushort)this.Reader.BaseStream.Length;
            }
        }


        internal virtual void Encrypt()
        {
            if (this.Device.State >= State.LOGGED)
            {
                this.Device.RNonce.Increment();

                this.Data = new List<byte>(Sodium.Encrypt(this.Data.ToArray(), this.Device.RNonce, this.Device.PublicKey).Skip(16).ToArray());
            }
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
            /*new Global_Chat_Entry(this.Device)
            {
                Message = message,
                Message_Sender = this.Device.Player.Avatar,
                Bot = true
            }.Send();*/
        }

        internal void ShowValues()
        {
            Console.WriteLine(Environment.NewLine);

            foreach (FieldInfo Field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (Field != null)
                {
                    Control.SayInfo(Utils.Padding(this.GetType().Name) + " - " + Utils.Padding(Field.Name) + " : " + Utils.Padding(!string.IsNullOrEmpty(Field.Name) ? (Field.GetValue(this) != null ? Field.GetValue(this).ToString() : "(null)") : "(null)", 40));
                }
            }
        }
    }
}