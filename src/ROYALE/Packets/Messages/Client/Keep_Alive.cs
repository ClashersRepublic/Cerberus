namespace CRepublic.Royale.Packets.Messages.Client
{
    using CRepublic.Royale.Core.Network;
    using CRepublic.Royale.Extensions.Binary;
    using CRepublic.Royale.Logic;
    using CRepublic.Royale.Packets.Messages.Server;

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
