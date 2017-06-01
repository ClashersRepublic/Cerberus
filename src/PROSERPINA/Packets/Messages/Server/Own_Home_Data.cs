using System;
using BL.Servers.CR.Extensions.List;
using BL.Servers.CR.Library.ZLib;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Messages.Server
{
    internal class Own_Home_Data : Message
    {
        internal Own_Home_Data(Device Device) : base(Device)
        {
            this.Identifier = 24101;
        }

        internal override void Encode()
        {
            this.Data.AddRange(this.Device.Player.Component.ToBytes);
            this.Data.AddRange(this.Device.Player.Profile.ToBytes);
        }

        internal override void Process()
        {
        }
    }
}
