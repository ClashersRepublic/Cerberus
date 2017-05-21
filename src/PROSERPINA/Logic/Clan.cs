using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Extensions.List;
using BL.Servers.CR.Logic.Enums;
using BL.Servers.CR.Logic.Slots;
using Newtonsoft.Json;

namespace BL.Servers.CR.Logic
{
    internal class Clan
    {
        [JsonProperty("clan_id")] internal long ClanID;

        [JsonProperty("name")] internal string Name;
        [JsonProperty("desc")] internal string Description;

        [JsonProperty("clan_badge")] internal int Badge;

        [JsonProperty("clan_type")] internal Hiring Type = Hiring.OPEN;

        [JsonProperty("req_score")] internal int Required_Score;

        [JsonProperty("clan_origin")] internal int Origin;

        [JsonProperty("members")] internal Members Members;

        internal int Trophies
        {
            get
            {
                return this.Members.Values.ToList().Sum(Member => (Member.Player.Avatar.Trophies / 2));
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

        internal byte[] FullHeader()
        {
            var Packet = new List<byte>();

            Packet.AddLong(this.ClanID);
            Packet.AddString(this.Name);
            Packet.AddVInt(16);
            Packet.AddVInt(this.Badge);
            Packet.AddVInt(1);
            Packet.AddVInt(0); // Member Count
            Packet.AddVInt(this.Trophies);
            Packet.AddVInt(this.Required_Score);
            Packet.AddLong(57);
            Packet.AddVInt(this.Origin);

            return Packet.ToArray();
        }

        internal byte[] Header()
        {
            var Packet = new List<byte>();

            return Packet.ToArray();
        }
    }
}
