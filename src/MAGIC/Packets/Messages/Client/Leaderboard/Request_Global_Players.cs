using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Packets.Messages.Server.Leaderboard;

namespace CRepublic.Magic.Packets.Messages.Client.Leaderboard
{
    internal class Request_Global_Players :Message
    {
        public Request_Global_Players(Device device) : base(device)
        {
        }
        internal override void Process()
        {
            if (this.Device.Player.Avatar.Variables.IsBuilderVillage)
            {
                
            }
            else
            {
                new Global_Players(this.Device).Send();
            }
        }
    }
}
