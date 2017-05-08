using BL.Servers.CoC.Logic;

namespace BL.Servers.CoC.Packets.Messages.Server
{
    internal class Keep_Alive_OK : Message
    {
        internal Keep_Alive_OK(Device Device) : base(Device)
        {
            this.Identifier = 20108;
        }
    }
}