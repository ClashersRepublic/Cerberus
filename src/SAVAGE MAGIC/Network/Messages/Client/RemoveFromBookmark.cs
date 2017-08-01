using System;
using System.IO;

using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Logic.DataSlots;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class RemoveFromBookmarkMessage : Message
    {
        private long id;

        public RemoveFromBookmarkMessage(ClashOfClans.Client client, PacketReader br) : base(client, br)
        {
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
            BookmarkSlot al = level.Avatar.BookmarkedClan.Find((Predicate<BookmarkSlot>)(a => a.Value == id));
            if (al != null)
                level.Avatar.BookmarkedClan.Remove(al);
            new BookmarkRemoveAllianceMessage(Client).Send();
        }
    }
}