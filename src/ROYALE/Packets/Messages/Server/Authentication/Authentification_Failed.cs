using CRepublic.Royale.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CRepublic.Royale.Extensions.List;
using CRepublic.Royale.Library.Blake2B;
using CRepublic.Royale.Library.Sodium;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Logic.Enums;
using CRepublic.Royale.Packets.Cryptography;
using CRepublic.Royale.Files;
using CRepublic.Royale.Core;

namespace CRepublic.Royale.Packets.Messages.Server.Authentication
{
    internal class Authentification_Failed : Message
    {
        internal string UpdateURL = "https://barbarianland.com";
        internal string RedirectURL = "https://barbarianland.com";

        internal string Message = string.Empty;

        internal LoginFailed_Reason Reason;

        /// <summary>
        /// Initializes a new instance of the <see cref="Authentification_Failed"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Reason">The reason.</param>
        internal Authentification_Failed(Device Device, LoginFailed_Reason Reason = LoginFailed_Reason.Default) : base(Device)
        {
            this.Identifier = 20103;
            this.Reason = Reason;
            this.Version = 2;
        }
            
        internal string ContentURL => Fingerprint.Custom ? "http://192.168.0.5/" : "http://b46f744d64acd2191eda-3720c0374d47e9a0dd52be4d281c260f.r11.cf2.rackcdn.com/";

        internal override void Encode()
        {
            this.Data.AddVInt((int)this.Reason);
            this.Data.AddString(Fingerprint.Json);
            this.Data.AddString(null);
            this.Data.AddString(ContentURL);
            this.Data.AddString(null);
            this.Data.AddString(this.Message);
            this.Data.AddVInt(this.Reason == LoginFailed_Reason.Maintenance ? Constants.Maintenance_Timer.GetRemainingSeconds(DateTime.Now) : 0);
            this.Data.AddByte(0);
            this.Data.AddString(null);
        }

        /// <summary>
        /// Encrypts this message.
        /// </summary>
        internal override void EncryptSodium()
        {
            if (this.Device.PlayerState >= Client_State.LOGIN)
            {
                Blake2BHasher Blake = new Blake2BHasher();

                Blake.Update(this.Device.Crypto.SNonce);
                Blake.Update(this.Device.Crypto.PublicKey);
                Blake.Update(Key.Crypto.PublicKey);

                byte[] Nonce = Blake.Finish();
                byte[] Encrypted = this.Device.Crypto.RNonce.Concat(this.Device.Crypto.PublicKey).Concat(this.Data).ToArray();

                this.Data = new List<byte>(Sodium.Encrypt(Encrypted, Nonce, Key.Crypto.PrivateKey, this.Device.Crypto.PublicKey));
            }

            this.Length = (ushort)this.Data.Count;
        }
    }
}
