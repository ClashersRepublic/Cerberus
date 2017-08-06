using System.Collections.Generic;
using System.Linq;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.List;
using CRepublic.Magic.External.Sodium;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;

namespace CRepublic.Magic.Packets.Messages.Server.Authentication
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
    }
}
