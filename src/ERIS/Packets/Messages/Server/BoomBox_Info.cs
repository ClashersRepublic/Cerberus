using System;
namespace BL.Servers.BB.Packets.Messages.Server
{
    using BL.Servers.BB.Extensions.List;
    using BL.Servers.BB.Logic;

    internal class BoomBox_Info :Message
    {
        internal BoomBox_Info(Device Device) : base(Device)
        {
            this.Identifier = 24448;
        }

        internal override void Encode()
        {
            this.Data.AddString("Hi bitch");
        }
    }
}
