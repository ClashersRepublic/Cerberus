using System.Collections.Generic;
using System.Linq;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.External.Sodium;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;

namespace BL.Servers.CoC.Packets.Messages.Server.Authentication
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

        internal override void Encrypt()
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
