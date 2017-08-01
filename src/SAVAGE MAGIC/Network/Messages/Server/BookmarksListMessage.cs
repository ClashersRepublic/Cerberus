using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Logic.DataSlots;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    // Packet 24341
    internal class BookmarkListMessage : Message
    {
        public Avatar player { get; set; }
        public int i;

        public BookmarkListMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 24341;
            player = client.Level.Avatar;
            i = 0;
        }

        public override void Encode()
        {
            var data = new List<byte>();
            var list = new List<byte>();
            var rem = new List<BookmarkSlot>();
            Parallel.ForEach((player.BookmarkedClan), (p, l) =>
            {
                Alliance a = ObjectManager.GetAlliance(p.Value);
                if (a != null)
                {
                    list.AddRange(ObjectManager.GetAlliance(p.Value).EncodeFullEntry());
                    i++;
                }
                else
                {
                    rem.Add(p);
                    if (i > 0)
                        i--;
                }
                l.Stop();
            });
            data.AddInt32(i);
            data.AddRange(list);
            Encrypt(data.ToArray());
            Parallel.ForEach((rem), (im, l) =>
            {
                player.BookmarkedClan.RemoveAll(t => t == im);
                l.Stop();
            });
        }
    }
}
