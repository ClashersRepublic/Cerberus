using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Extensions.List;
using CRepublic.Royale.Logic.Enums;
using CRepublic.Royale.Logic.Slots;
using Newtonsoft.Json;
using CRepublic.Royale.Logic.Slots.Items;

namespace CRepublic.Royale.Logic
{
    internal class Clan
    {
        [JsonProperty("clan_id")] internal long ClanID;

        [JsonProperty("name")] internal string Name;
        [JsonProperty("desc")] internal string Description;

        [JsonProperty("clan_badge")] internal int Badge;

        [JsonProperty("clan_type")] internal Clan_Type Type = Clan_Type.OPEN;

        [JsonProperty("req_score")] internal int Required_Score;

        [JsonProperty("clan_origin")] internal int Origin;

        [JsonProperty("members")] internal Members Members;

        internal int Trophies
        {
            get
            {
                return this.Members.Values.ToList().Sum(Member => (Member.Player.Trophies / 2));
            }
        }

        internal Clan()
        {
            this.Members = new Members(this);
        }

        internal Clan(long ClanID = 0)
        {
            this.ClanID = ClanID;

            this.Members = new Members(this);
        }

        internal byte[] FullHeader
        {
            get
            {
                var Packet = new List<byte>();

                Packet.AddLong(this.ClanID);
                Packet.AddString(this.Name);
                Packet.AddVInt(16);
                Packet.AddVInt(this.Badge);
                Packet.AddVInt(1);
                Packet.AddVInt(this.Members.Count); // Member Count
                Packet.AddVInt(this.Trophies);
                Packet.AddVInt(this.Required_Score);
                Packet.AddLong(57);
                Packet.AddVInt(this.Origin);

                return Packet.ToArray();
            }
        }
    }
}
