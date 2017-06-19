using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Extensions;
using CRepublic.Royale.Extensions.List;
using CRepublic.Royale.Library.Sodium;
using CRepublic.Royale.Logic;

namespace CRepublic.Royale.Packets.Messages.Server.Authentication
{
    internal class Unlock_Account_OK : Message
    {
        internal Player Player;
        internal Unlock_Account_OK(Device Device) : base(Device)
        {
            this.Identifier = 20132;
        }

        internal override void Encode()
        {
            this.Data.AddLong(this.Player.UserId);
            this.Data.AddString(this.Player.Token);
        }

        internal override void EncryptSodium()
        {
            this.Device.Crypto.RNonce.Increment();

            this.Data = new List<byte>(Sodium
                .Encrypt(this.Data.ToArray(), this.Device.Crypto.RNonce, this.Device.Crypto.PublicKey)
                .Skip(16)
                .ToArray());

            this.Length = (ushort)this.Data.Count;
        }
    }
}
