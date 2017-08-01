using Magic.ClashOfClans.Network;

namespace Magic.Packets.Messages.Client
{
    internal class AddClashFriendMessage : Message
    {
        public long FriendId;

        public AddClashFriendMessage(ClashOfClans.Client client, PacketReader br) : base(client, br)
        {
            // Space
        }
    }
}
