using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BL.Servers.CoC.Core;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.External.Sodium;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Packets;
using BL.Servers.CoC.Packets.Messages.Server.Authentication;

namespace BL.Servers.CoC.Packets.Client.Authentication
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
                  if (int.TryParse(this.UnlockCode.Substring(1), out int n))
                  {
                      var account = Resources.Players.Get(n);
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
                new Unlock_Account_OK(this.Device) {Account = this.Device.Player.Avatar}.Send();
            }
            else
            {
                new Unlock_Account_Failed(this.Device) {Reason = UnlockReason.Default}.Send();

            }
        }

        internal override void Decrypt()
        {
            var data = this.Reader.ReadBytes(this.Length);
            this.Device.Keys.SNonce.Increment();
            Console.WriteLine(BitConverter.ToString(data.ToArray()));
            Console.WriteLine(Encoding.UTF8.GetString(data.ToArray()));
            byte[] Decrypted = Sodium.Decrypt(data.ToArray(), this.Device.Keys.SNonce, this.Device.Keys.PublicKey);

            if (Decrypted == null)
            {
                throw new CryptographicException("Tried to decrypt an incomplete message.");
            }

            this.Reader = new Reader(Decrypted);
            this.Length = (ushort) this.Reader.BaseStream.Length;
        }
    }
}