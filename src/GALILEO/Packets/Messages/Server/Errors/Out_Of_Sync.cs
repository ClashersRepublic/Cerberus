using Republic.Magic.Logic;

namespace Republic.Magic.Packets.Messages.Server.Errors
{
    internal class Out_Of_Sync : Message
    {
        public Out_Of_Sync(Device Device) : base(Device)
        {
            this.Identifier = 24104;
        }
    }
}