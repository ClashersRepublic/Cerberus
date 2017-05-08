namespace BL.Servers.CR.Packets.Messages.Client
{
    using BL.Servers.CR.Core.Network;
    using BL.Servers.CR.Extensions.Binary;
    using BL.Servers.CR.Logic;
    using BL.Servers.CR.Packets.Messages.Server;

    internal class Keep_Alive : Message
    {
        public Keep_Alive(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Keep_Alive.
        }

        internal override void Process()
        {
            new Keep_Alive_Response(this.Device).Send();
        }
    }
}
