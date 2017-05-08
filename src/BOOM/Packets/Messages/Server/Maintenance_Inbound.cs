using BL.Servers.BB.Logic;

namespace BL.Servers.BB.Packets.Messages.Server
{
    using BL.Servers.BB.Extensions.List;

    internal class Maintenance_Inbound : Message
    {
        internal int SecondsTillMaintenance;
        internal Maintenance_Inbound(Device Device, int seconds) : base(Device)
        {
            this.Identifier = 20171;
            this.SecondsTillMaintenance = seconds;
        }

        internal override void Encode()
        {
            this.Data.AddInt(this.SecondsTillMaintenance);
        }
    }
}
