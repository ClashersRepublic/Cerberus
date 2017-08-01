using System.IO;
using Savage.Magic.Core;

using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Logic.DataSlots;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.Messages.Client
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