using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.BB.Core;
using BL.Servers.BB.Core.Network;
using BL.Servers.BB.Extensions;
using BL.Servers.BB.Extensions.Binary;
using BL.Servers.BB.Library.Sodium;
using BL.Servers.BB.Logic;
using BL.Servers.BB.Logic.Enums;
using BL.Servers.BB.Packets.Messages.Server;
using BL.Servers.BB.Packets.Messages.Server.Authentication;

namespace BL.Servers.BB.Packets.Messages.Client.Authentication
{
    internal class Unlock_Account : Message
    {
        internal long UserID;
        internal string UserToken;
        internal string UnlockCode;
        internal string UserPassword;
        public Unlock_Account(Device Device, Reader Reader) : base(Device, Reader)
        {
            this.UserPassword = this.Device.Player.Avatar.Password;
        }

        internal override void Decode()
        {
            this.UserID = this.Reader.ReadInt64();
            this.UserToken = this.Reader.ReadString();

            this.UnlockCode = this.Reader.ReadString();
        }

        internal override void Process()
        {
            this.ShowValues();

            if (this.UnlockCode.Length != 12 || string.IsNullOrEmpty(this.UnlockCode))
            {
                Resources.Devices.Remove(this.Device);
                return;
            }

            if (this.UnlockCode[0] == '/')
            {
                //if (int.TryParse(this.UnlockCode.Substring(1), out int n))
                //{
                //    var account = Resources.Players.Get(n);
                //    if (account != null)
                //    {
                //        account.Avatar.Locked = true;
                //        new Unlock_Account_OK(this.Device) {Account = account.Avatar}.Send();
                //    }
                //    else
                //    {
                //        new Unlock_Account_Failed(this.Device) {Reason = UnlockReason.UnlockError}.Send();
                //    }
                //}
                //else
                //{
                //    new Unlock_Account_Failed(this.Device) {Reason = UnlockReason.UnlockError}.Send();
                //}
            }
            if (string.Equals(this.UnlockCode, this.UserPassword))
            {
                this.Device.Player.Avatar.Locked = false;
                new Unlock_Account_OK(this.Device) {Account = this.Device.Player.Avatar}.Send();
            }
            else
            {
                new Unlock_Account_Failed(this.Device) {Reason = UnlockReason.Default}.Send();

            }
        }

        internal override void Decrypt()
        {
            this.Device.Keys.SNonce.Increment();

            byte[] Decrypted = Sodium.Decrypt(new byte[16].Concat(this.Reader.ReadBytes(this.Length)).ToArray(), this.Device.Keys.SNonce, this.Device.Keys.PublicKey);

            if (Decrypted == null)
            {
                throw new CryptographicException("Tried to decrypt an incomplete message.");
            }

            this.Reader = new Reader(Decrypted);
            this.Length = (ushort)this.Reader.BaseStream.Length;
        }
    }
}
