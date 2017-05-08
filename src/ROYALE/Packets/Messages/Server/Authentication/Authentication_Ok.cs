using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Extensions.List;
using BL.Servers.CR.Library.Blake2B;
using BL.Servers.CR.Library.Sodium;
using BL.Servers.CR.Logic;
using BL.Servers.CR.Logic.Enums;

namespace BL.Servers.CR.Packets.Messages.Server.Authentication
{
    internal class Authentification_OK : Message
    {
        internal Authentification_OK(Device Device) : base(Device)
        {
            this.Identifier = 20104;
            this.Version = 1;
            this.Device.PlayerState = State.LOGGED;
        }

        internal override void Encode()
        {
            this.Data.AddLong(this.Device.Player.Avatar.UserId);
            this.Data.AddLong(this.Device.Player.Avatar.UserId);

            this.Data.AddString(this.Device.Player.Avatar.Token);

            this.Data.AddString(null); // Facebook
            this.Data.AddString(null); // Gamecenter

            this.Data.AddVInt(3); // VInt
            this.Data.AddVInt(193); // VInt
            this.Data.AddVInt(6); // VInt

            this.Data.AddString("prod");


            this.Data.AddVInt(0); // VInt
            this.Data.AddVInt(0); // VInt
            this.Data.AddVInt(0); // VInt

            this.Data.AddString("1475268786112433");
            this.Data.AddString(null);
            this.Data.AddString(null);
            this.Data.AddVInt(0); // VInt

            this.Data.AddString("1493262974759");
            this.Data.AddString("1493262332000");
            this.Data.AddString(null);

            this.Data.AddString("CA");

            this.Data.AddString("http://7166046b142482e67b30-2a63f4436c967aa7d355061bd0d924a1.r65.cf1.rackcdn.com");
            this.Data.AddString("https://event-assets.clashroyale.com");

            this.Data.AddByte(1);

        }
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
