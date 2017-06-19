using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Logic.Slots.Items;
using Newtonsoft.Json;

namespace CRepublic.Royale.Logic.Slots
{
    internal class Chats
    {
        [JsonProperty("seed")] internal int Seed;
        [JsonProperty("slots")] internal List<Logic.Slots.Items.Entry> Slots;

        internal object Gate = new object();
        internal Clan Clan;

        internal Chats()
        {
            this.Slots = new List<Logic.Slots.Items.Entry>(100);
        }

        internal Chats(Clan Clan, int Limit = 100)
        {
            this.Clan = Clan;
            this.Slots = new List<Logic.Slots.Items.Entry>(Limit);
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> Packet = new List<byte>();



                return Packet.ToArray();
            }
        }
    }
}
