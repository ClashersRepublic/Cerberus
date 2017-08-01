using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CRepublic.Magic.Core;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.External.Sodium;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Packets;
using CRepublic.Magic.Packets.Messages.Server.Authentication;

namespace CRepublic.Magic.Packets.Client.Authentication
{
    internal class Unlock_Account : Message
    {
        internal long UserID;
        internal string UserToken;
        internal string UnlockCode;
        internal string UserPassword;

        public Unlock_Account(Device Device, Reader Reader) : base(Device, Reader)
        {
            this.UserPassword = this.Device.Player != null ? this.Device.Player.Avatar.Password : String.Empty;
        }

        internal override void Decode()
        {
            this.UserID = this.Reader.ReadInt64();
            this.UserToken = this.Reader.ReadString();

            this.UnlockCode = this.Reader.ReadString();
        }

        internal override void Process()
        {
            if (this.UnlockCode.Length != 12 || string.IsNullOrEmpty(this.UnlockCode))
            {
                Devices.Remove(this.Device.SocketHandle);
                return;
            }

            if (this.UnlockCode[0] == '/')
            {
                int n = 0;
                if (int.TryParse(this.UnlockCode.Substring(1), out n))
                {
                    if (n == 0)
                    {
                        new Unlock_Account_OK(this.Device) { Account = Players.New().Avatar }.Send();
                        return;
                    }

                    var account = Players.Get(n);
                    if (account != null)
                    {
                        account.Avatar.Locked = true;
                        new Unlock_Account_OK(this.Device) { Account = account.Avatar }.Send();
                    }
                    else
                    {
                        new Unlock_Account_Failed(this.Device) { Reason = UnlockReason.UnlockError }.Send();
                    }

                }
                else
                {
                    new Unlock_Account_Failed(this.Device) { Reason = UnlockReason.UnlockError }.Send();
                }
            }
            if (string.Equals(this.UnlockCode, this.UserPassword))
            {
                this.Device.Player.Avatar.Locked = false;
                new Unlock_Account_OK(this.Device) { Account = this.Device.Player.Avatar }.Send();
            }
            else
            {
                new Unlock_Account_Failed(this.Device) { Reason = UnlockReason.Default }.Send();

            }
        }

        internal override void DecryptPepper()
        {
            Console.WriteLine(BitConverter.ToString(this.Device.Keys.SNonce));
            //this.Device.Keys.SNonce.Increment(0);
            byte[] Decrypted = Sodium.Decrypt(new byte[16].Concat(this.Reader.ReadBytes((int)this.Length)).ToArray(), this.Device.Keys.SNonce, this.Device.Keys.PublicKey);

            if (Decrypted == null)
            {
                throw new CryptographicException("Tried to decrypt an incomplete message.");
            }

            this.Reader = new Reader(Decrypted);
            this.Length = (ushort)this.Reader.BaseStream.Length;
        }
    }
}