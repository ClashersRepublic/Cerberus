using System.Collections.Generic;
using System.Linq;
using Republic.Magic.Extensions;
using Republic.Magic.Extensions.List;
using Republic.Magic.External.Sodium;
using Republic.Magic.Logic;
using Republic.Magic.Logic.Enums;

namespace Republic.Magic.Packets.Messages.Server.Authentication
{
    internal class Unlock_Account_Failed : Message
    {
        internal UnlockReason Reason;
        internal Unlock_Account_Failed(Device Device) : base(Device)
        {
            this.Identifier = 20133;
            this.Version = 1;
        }

        internal override void Encode()
        {
            this.Data.AddInt((int)this.Reason);
        }

        internal override void EncryptPepper()
        {
            this.Device.Keys.RNonce.Increment();

            this.Data = new List<byte>(Sodium
                .Encrypt(this.Data.ToArray(), this.Device.Keys.RNonce, this.Device.Keys.PublicKey)
                .Skip(16)
                .ToArray());

            this.Length = (ushort)this.Data.Count;
        }
    }
}
