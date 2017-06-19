namespace CRepublic.Royale.Packets.Messages.Server
{
    using CRepublic.Royale.Logic;

    internal class Keep_Alive_Response : Message
    {
        internal Keep_Alive_Response(Device Device) : base(Device)
        {
            this.Identifier = 20108;
        }
    }
}
