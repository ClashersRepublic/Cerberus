using Savage.Magic;

namespace Savage.Magic.Network.Messages.Client
{
    internal class NewsSeenMessage : Message
    {
        public NewsSeenMessage(ClashOfClans.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
        }
    }
}