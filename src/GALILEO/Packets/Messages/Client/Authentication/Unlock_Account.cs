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
            this.UserPassword = this.Device.Player != null ? this.Device.Player.Avatar.Password : String.Empty;
        }

        internal override void Decode()
        {
            this.UserID = this.Reader.ReadInt64();
            this.UserToken = this.Reader.ReadString();

            this.UnlockCode = this.Reader.ReadString();
        }

        internal override async void Process()
        {
            if (this.UnlockCode.Length != 12 || string.IsNullOrEmpty(this.UnlockCode))
            {
                Resources.Devices.Remove(this.Device);
                return;
            }

             if (this.UnlockCode[0] == '/')
              {
                  if (int.TryParse(this.UnlockCode.Substring(1), out int n))
                  {
                      if (n == 0)
                      {
                          new Unlock_Account_OK(this.Device) {Account = Resources.Players.New().Avatar}.Send();
                          return;
                      }

                      var account = await Resources.Players.Get(n);
                      if (account != null)
                      {
                          account.Avatar.Locked = true;
                          new Unlock_Account_OK(this.Device) {Account = account.Avatar}.Send();
                      }
                      else
                      {
                          new Unlock_Account_Failed(this.Device) {Reason = UnlockReason.UnlockError}.Send();
                      }

                  }
                  else
                  {
                      new Unlock_Account_Failed(this.Device) {Reason = UnlockReason.UnlockError}.Send();
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

        internal override void DecryptPepper()
        {
            Console.WriteLine(BitConverter.ToString(this.Device.Keys.SNonce));
            //this.Device.Keys.SNonce.Increment(0);
            byte[] Decrypted = Sodium.Decrypt(new byte[16].Concat(this.Reader.ReadBytes(this.Length)).ToArray(), this.Device.Keys.SNonce, this.Device.Keys.PublicKey);

            if (Decrypted == null)
            {
                throw new CryptographicException("Tried to decrypt an incomplete message.");
            }

            this.Reader = new Reader(Decrypted);
            this.Length = (ushort) this.Reader.BaseStream.Length;
        }
    }
}