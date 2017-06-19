using Republic.Magic.Core.Networking;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Logic;
using Republic.Magic.Packets.Messages.Server.Leaderboard;

namespace Republic.Magic.Packets.Messages.Client.Leaderboard
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
