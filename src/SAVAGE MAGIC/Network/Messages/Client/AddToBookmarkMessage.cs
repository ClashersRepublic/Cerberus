using System.IO;
using Magic.ClashOfClans.Core;

using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Logic.DataSlots;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class AddToBookmarkMessage : Message
    {
        private long id;

        public AddToBookmarkMessage(ClashOfClans.Client client, PacketReader br) : base(client, br)
        {
            // Space
        }

        public override void Decode()
        {
            using (PacketReader br = new PacketReader(new MemoryStream(Data)))
            {
                id = br.ReadInt64();
            }
        }

        public override void Process(Level level)
        {
            level.Avatar.BookmarkedClan.Add(new BookmarkSlot(id));
            new BookmarkAddAllianceMessage(level.Client).Send();
        }
    }
}