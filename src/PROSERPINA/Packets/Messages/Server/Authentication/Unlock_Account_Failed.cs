namespace BL.Servers.CR.Packets.Messages.Server.Authentication
{
    using BL.Servers.CR.Extensions;
    using BL.Servers.CR.Extensions.List;
    using BL.Servers.CR.Library.Sodium;
    using BL.Servers.CR.Logic;
    using BL.Servers.CR.Logic.Enums;
    using System.Collections.Generic;
    using System.Linq;

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

        internal override void EncryptSodium()
        {
            this.Device.Crypto.RNonce.Increment();

            this.Data = new List<byte>(Sodium
                .Encrypt(this.Data.ToArray(), this.Device.Crypto.RNonce, this.Device.Crypto.PublicKey)
                .Skip(16)
                .ToArray());

            this.Length = (ushort) this.Data.Count;
        }
    }
}
