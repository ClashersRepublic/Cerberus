using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic; 

namespace BL.Servers.CoC.Packets.Messages.Client.Leaderboard
{
    internal class Request_Local_Players : Message
    {
        public Request_Local_Players(Device device, Reader reader) : base(device, reader)
        {
        }
        internal override void Process()
        {
            //new Local_Players(this.Device).Send();
        }
    }
}
