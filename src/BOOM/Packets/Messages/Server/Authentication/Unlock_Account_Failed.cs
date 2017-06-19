namespace CRepublic.Boom.Packets.Messages.Server.Authentication
{
    using CRepublic.Boom.Extensions;
    using CRepublic.Boom.Extensions.List;
    using CRepublic.Boom.Library.Sodium;
    using CRepublic.Boom.Logic;
    using CRepublic.Boom.Logic.Enums;
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

        internal override void Encrypt()
        {
            this.Device.Keys.RNonce.Increment();

            this.Data = new List<byte>(Sodium
                .Encrypt(this.Data.ToArray(), this.Device.Keys.RNonce, this.Device.Keys.PublicKey)
                .Skip(16)
                .ToArray());

            this.Length = (ushort) this.Data.Count;
        }
    }
}
