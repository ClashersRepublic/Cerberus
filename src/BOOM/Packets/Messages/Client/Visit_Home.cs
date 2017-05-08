namespace BL.Servers.BB.Packets.Messages.Client
{
    using BL.Servers.BB.Core.Network;
    using BL.Servers.BB.Extensions.Binary;
    using BL.Servers.BB.Logic;
    using BL.Servers.BB.Packets.Messages.Server;

    internal class Visit_Home : Message
    {
        internal string Name;
        public Visit_Home(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override void Process()
        {
            new Own_Home_Data(this.Device).Send();
            new Shutdown_Started(this.Device, 100).Send();
        }
    }
}
