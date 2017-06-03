using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Packets.Messages.Server.Leaderboard;

namespace BL.Servers.CoC.Packets.Messages.Client.Leaderboard
{
    internal class Request_League_Players : Message
    { 
        public Request_League_Players(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override void Decode()
        {
        }

        internal override void Process()
        {
            //new League_Players(this.Device).Send();
        }
    }
}
