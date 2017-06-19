using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Boom.Extensions.List;
using CRepublic.Boom.Library.Blake2B;
using CRepublic.Boom.Library.Sodium;
using CRepublic.Boom.Logic;
using CRepublic.Boom.Logic.Enums;

namespace CRepublic.Boom.Packets.Messages.Server.Authentication
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

            this.Data.AddString(string.Empty); // Facebook
            this.Data.AddString(string.Empty); // Gamecenter

            this.Data.AddInt(30);
            this.Data.AddInt(133);
            this.Data.AddInt(1);

            this.Data.AddString("prod");


            this.Data.AddString("480068372075366"); // 297484437009394

            this.Data.AddString("MY"); // 14 75 26 87 86 11 24 33
            this.Data.AddString("1492251603079"); // 14 78 03 95 03 10 0

            this.Data.AddString(string.Empty); // 14 78 03 95 03 10 0

            //0 mean none
            //1 mean tid
            //2 mean event
            //3 mean both // Why event is not compressed in this state.. Is there 4?
            this.Data.AddByte(0);
  
            this.Data.AddString("1482970881296"); // 14 75 26 87 86 11 24 33

            this.Data.AddInt(0); // 14 75 26 87 86 11 24 33
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
