using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Packets.Messages.Server.Leaderboard;

namespace BL.Servers.CoC.Packets.Messages.Client.Leaderboard
{
    internal class Request_Local_Clans : Message
    {
        public Request_Local_Clans(Device Device) : base(Device)
        {

        }

        internal override void Decode()
        {
        }

        internal override void Process()
        {
            //new Local_Clans(this.Device).Send();
        }
    }
}
