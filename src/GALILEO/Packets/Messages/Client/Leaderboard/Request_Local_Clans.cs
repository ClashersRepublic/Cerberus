using Republic.Magic.Core.Networking;
using Republic.Magic.Logic;
using Republic.Magic.Packets.Messages.Server.Leaderboard;

namespace Republic.Magic.Packets.Messages.Client.Leaderboard
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
