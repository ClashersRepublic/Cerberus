using Republic.Magic.Core.Networking;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Logic; 

namespace Republic.Magic.Packets.Messages.Client.Leaderboard
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
