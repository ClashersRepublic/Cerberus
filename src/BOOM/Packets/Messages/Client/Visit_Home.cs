namespace CRepublic.Boom.Packets.Messages.Client
{
    using CRepublic.Boom.Core.Network;
    using CRepublic.Boom.Extensions.Binary;
    using CRepublic.Boom.Logic;
    using CRepublic.Boom.Packets.Messages.Server;

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
