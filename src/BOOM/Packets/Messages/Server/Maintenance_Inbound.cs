using CRepublic.Boom.Logic;

namespace CRepublic.Boom.Packets.Messages.Server
{
    using CRepublic.Boom.Extensions.List;

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
