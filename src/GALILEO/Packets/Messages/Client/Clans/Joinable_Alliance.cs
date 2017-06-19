using Republic.Magic.Logic;
using Republic.Magic.Core.Networking;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Packets.Messages.Server.Clans;

namespace Republic.Magic.Packets.Messages.Client.Clans
{
    internal class Joinable_Alliance : Message
    {
        public Joinable_Alliance(Device Device, Reader Reader) : base(Device, Reader)
        {

        }

        internal override void Process()
        {
            new Alliance_Joinable_Data(this.Device).Send();
        }
    }
}
