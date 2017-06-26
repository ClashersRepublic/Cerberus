using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.List;
using CRepublic.Magic.External.Blake2B;
using CRepublic.Magic.External.Sodium;
using CRepublic.Magic.Files;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Packets.Cryptography;

namespace CRepublic.Magic.Packets.Messages.Server.Authentication
{
    internal class Authentication_Failed : Message
    {
        public Authentication_Failed(Device Device, Reason Reason = Reason.Default) : base(Device)
        {
            this.Identifier = 20103;
            this.Reason = Reason;
            this.Version = 9;
        }


        internal Reason Reason = Reason.Default;
        internal string PatchingHost => Fingerprint.Custom ? Constants.PatchServer : "https://www.clashersrepublic.com/game-content/projectmagic/";

        internal string Message;
        internal string RedirectDomain;

        internal override void Encode()
        {
            this.Data.AddInt((int)this.Reason);
            this.Data.AddString(this.Reason == Reason.Patch ? Fingerprint.Json : null);
            this.Data.AddString(this.RedirectDomain);
            this.Data.AddString(this.PatchingHost);
            this.Data.AddString(Constants.UpdateServer);
            this.Data.AddString(this.Message);
            this.Data.AddInt(this.Reason == Reason.Maintenance? Constants.Maintenance.GetRemainingSeconds(DateTime.Now) : 0);
            this.Data.AddByte(0);
            this.Data.AddCompressed(this.Reason == Reason.Patch ? Fingerprint.Json : null, false);
            this.Data.AddInt(-1);
            this.Data.AddInt(2);
            this.Data.AddInt(0);
            this.Data.AddInt(-1);
        }

        internal override void EncryptPepper()
        {
            if (this.Device.State >= State.LOGIN)
            {
                Blake2BHasher blake = new Blake2BHasher();

                blake.Update(this.Device.Keys.SNonce);
                blake.Update(this.Device.Keys.PublicKey);
                blake.Update(Key.PublicKey);

                byte[] Nonce = blake.Finish();
                byte[] encrypted = this.Device.Keys.RNonce.Concat(this.Device.Keys.PublicKey).Concat(this.Data).ToArray();

                this.Data = new List<byte>(Sodium.Encrypt(encrypted, Nonce, Key.PrivateKey, this.Device.Keys.PublicKey));
            }

            this.Length = (ushort)this.Data.Count;
        }
    }
}
