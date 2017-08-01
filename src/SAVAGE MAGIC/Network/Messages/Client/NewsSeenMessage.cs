using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class NewsSeenMessage : Message
    {
        public NewsSeenMessage(ClashOfClans.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
        }
    }
}