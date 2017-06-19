using Republic.Magic.Logic;

namespace Republic.Magic.Packets.Messages.Server
{
    internal class Keep_Alive_OK : Message
    {
        internal Keep_Alive_OK(Device Device) : base(Device)
        {
            this.Identifier = 20108;
        }
    }
}