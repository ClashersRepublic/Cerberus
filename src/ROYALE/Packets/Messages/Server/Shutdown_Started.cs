using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Messages.Server
{
    using BL.Servers.CR.Extensions.List;

    internal class Shutdown_Started : Message
    {
        internal Shutdown_Started(Device Device) : base(Device)
        {
            this.Identifier = 20161;
        }

        internal override void Encode()
        {
            this.Data.AddVInt(2);
        }
    }
}
