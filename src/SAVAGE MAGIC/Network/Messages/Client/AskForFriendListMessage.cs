using System.IO;

using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.Messages.Client
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