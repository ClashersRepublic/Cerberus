namespace BL.Servers.CR.Packets.Messages.Server
{
    using BL.Servers.CR.Logic;

    internal class Keep_Alive_Response : Message
    {
        internal Keep_Alive_Response(Device Device) : base(Device)
        {
            this.Identifier = 20108;
        }
    }
}
