using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.External.Blake2B;
using BL.Servers.CoC.External.Sodium;
using BL.Servers.CoC.Files;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Packets.Cryptography;

namespace BL.Servers.CoC.Packets.Messages.Server.Authentication
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
        internal string PatchingHost => Fingerprint.Custom ? "http://gamea.clashofclans.com/" : "http://b46f744d64acd2191eda-3720c0374d47e9a0dd52be4d281c260f.r11.cf2.rackcdn.com/";

        internal string Message;
        internal string RedirectDomain;

        internal int Time;

        internal override void Encode()
        {
            this.Data.AddInt((int)this.Reason);
            this.Data.AddString(Fingerprint.Json);
            this.Data.AddString(this.RedirectDomain);
            this.Data.AddString(this.PatchingHost);
            this.Data.AddString(Constants.UpdateServer);
            this.Data.AddString(this.Message);
            this.Data.AddInt(this.Time);
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
