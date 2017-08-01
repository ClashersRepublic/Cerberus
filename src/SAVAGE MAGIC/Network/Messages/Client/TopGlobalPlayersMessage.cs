using System.IO;

using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    // Packet 14403
    internal class TopGlobalPlayersMessage : Message
    {
        public TopGlobalPlayersMessage(ClashOfClans.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
        }

        public override void Process(Level level)
        {
            new GlobalPlayersMessage(Client).Send();
        }
    }
}
