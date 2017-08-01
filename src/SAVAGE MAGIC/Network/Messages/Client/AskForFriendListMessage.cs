using System.IO;

using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class AskForFriendListMessage : Message
    {
        public AskForFriendListMessage(ClashOfClans.Client client, PacketReader br)
            : base(client, br)
        {
            // Space
        }

        public override void Process(Level level)
        {
            new FriendListMessage(Client).Send();
        }
    }
}