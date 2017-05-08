using BL.Servers.BB.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL.Servers.BB.Extensions.List;
using BL.Servers.BB.Library.Blake2B;
using BL.Servers.BB.Library.Sodium;
using BL.Servers.BB.Logic;
using BL.Servers.BB.Logic.Enums;

namespace BL.Servers.BB.Packets.Messages.Server.Authentication
{
    internal class Authentification_Failed : Message
    {
        internal string UpdateHost = "http://df70a89d32075567ba62-1e50fe9ed7ef652688e6e5fff773074c.r40.cf1.rackcdn.com/";

        internal string Message = string.Empty;

        internal Reason Reason;

        /// <summary>
        /// Initializes a new instance of the <see cref="Authentification_Failed"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Reason">The reason.</param>
        internal Authentification_Failed(Device Device, Reason Reason = Reason.Default) : base(Device)
        {
            this.Identifier = 20103;
            this.Reason = Reason;
            this.Version = 9;
        }

        internal string PatchingHost => /*Fingerprint.Custom ? "bl" :*/ "http://b46f744d64acd2191eda-3720c0374d47e9a0dd52be4d281c260f.r11.cf2.rackcdn.com/";

        internal override void Encode()
        {
            this.Data.AddInt((int)this.Reason);
            this.Data.AddString(null); //Finger
            this.Data.AddString(this.PatchingHost);
            this.Data.AddString(this.Reason != Reason.Patch ? this.UpdateHost : null);
            this.Data.AddString("stage.clashofclans.com");
            this.Data.AddString(this.Message);
            
            this.Data.AddInt((int)TimeSpan.FromHours(3).TotalMilliseconds);
            this.Data.AddBool(false); //Show contact if banned
        }

        /// <summary>
        /// Encrypts this message.
        /// </summary>
        internal override void Encrypt()
        {
            if (this.Device.PlayerState >= State.LOGIN)
            {
                Blake2BHasher Blake = new Blake2BHasher();

                Blake.Update(this.Device.Keys.SNonce);
                Blake.Update(this.Device.Keys.PublicKey);
                Blake.Update(Key.Crypto.PublicKey);

                byte[] Nonce = Blake.Finish();
                byte[] Encrypted = this.Device.Keys.RNonce.Concat(this.Device.Keys.PublicKey).Concat(this.Data).ToArray();

                this.Data = new List<byte>(Sodium.Encrypt(Encrypted, Nonce, Key.Crypto.PrivateKey, this.Device.Keys.PublicKey));
            }

            this.Length = (ushort)this.Data.Count;
        }
    }
}
