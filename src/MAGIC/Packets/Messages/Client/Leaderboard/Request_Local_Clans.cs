using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Packets.Messages.Server.Leaderboard;

namespace CRepublic.Magic.Packets.Messages.Client.Leaderboard
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
