using System.IO;
using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.Messages.Client
{
    // Packet 14406
    internal class TopPreviousGlobalPlayersMessage : Message
    {
        public TopPreviousGlobalPlayersMessage(ClashOfClans.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
        }

        public override void Process(Level level)
        {
            new PreviousGlobalPlayersMessage(Client).Send();
        }
    }
}