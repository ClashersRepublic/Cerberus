using System.Collections.Generic;
using System.Linq;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.External.Sodium;
using BL.Servers.CoC.Logic;

namespace BL.Servers.CoC.Packets.Messages.Server.Authentication
{
    internal class Unlock_Account_OK : Message
    {
        internal Player Account;
        internal Unlock_Account_OK(Device Device) : base(Device)
        {
            this.Identifier = 20132;
        }

        internal override void Encode()
        {
            this.Data.AddLong(this.Account.UserId);
            this.Data.AddString(this.Account.Token);
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
