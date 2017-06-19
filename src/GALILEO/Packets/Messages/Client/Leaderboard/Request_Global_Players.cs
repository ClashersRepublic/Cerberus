using Republic.Magic.Core.Networking;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Logic;
using Republic.Magic.Packets.Messages.Server.Leaderboard;

namespace Republic.Magic.Packets.Messages.Client.Leaderboard
{
    internal class Request_Global_Players :Message
    {
        public Request_Global_Players(Device device, Reader reader) : base(device, reader)
        {
        }
        internal override void Process()
        {
            //new Global_Players(this.Device).Send();
        }
    }
}
