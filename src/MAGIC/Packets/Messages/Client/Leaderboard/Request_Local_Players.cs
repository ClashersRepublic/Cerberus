using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic; 

namespace CRepublic.Magic.Packets.Messages.Client.Leaderboard
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
