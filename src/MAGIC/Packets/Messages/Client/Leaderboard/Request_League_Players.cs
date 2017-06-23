using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Packets.Messages.Server.Leaderboard;

namespace CRepublic.Magic.Packets.Messages.Client.Leaderboard
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
            new League_Players(this.Device).Send();
        }
    }
}
