using BL.Servers.BB.Logic;

namespace BL.Servers.BB.Packets.Messages.Server
{
    using BL.Servers.BB.Extensions.List;

    internal class Shutdown_Started : Message
    {
        internal int SecondsTillShutdown;
        internal Shutdown_Started(Device Device, int seconds) : base(Device)
        {
            this.Identifier = 20161;
            this.SecondsTillShutdown = seconds;
        }

        internal override void Encode()
        {
            this.Data.AddInt(this.SecondsTillShutdown);
        }
    }
}
