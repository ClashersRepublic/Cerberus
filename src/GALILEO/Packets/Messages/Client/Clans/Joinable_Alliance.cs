using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Packets.Messages.Server.Clans;

namespace BL.Servers.CoC.Packets.Messages.Client.Clans
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
