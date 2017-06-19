using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Extensions.List;
using CRepublic.Royale.Logic.Enums;
using Newtonsoft.Json;

namespace CRepublic.Royale.Logic.Slots.Items
{
    internal class Entry
    {

        [JsonProperty("sender_id")] internal long Sender_ID;
        [JsonProperty("sender_name")] internal string Sender_Name;

        [JsonProperty("sender_arena")] internal int Sender_Arena;

        [JsonProperty("sender_level")] internal int Level;
        [JsonProperty("sender_score")] internal int Score;
        [JsonProperty("sender_donations")] internal int Donations;

        [JsonProperty("sender_crank")] internal byte CurrentRank;
        [JsonProperty("sender_prank")] internal byte PreviousRank;

        [JsonProperty("sender_cchest")] internal int CrownChestCrowns;

        [JsonProperty("sender_facebook")] internal string Sender_Facebook;

        [JsonProperty("sender_role")] internal Alliance_Role Sender_Role;


        internal byte[] ToBytes()
        {
            List<byte> _Packet = new List<byte>();

            _Packet.AddLong(this.Sender_ID);
            _Packet.AddString(this.Sender_Facebook);
            _Packet.AddString(this.Sender_Name);
            _Packet.AddVInt(this.Sender_Arena);
            _Packet.AddVInt((int) this.Sender_Role);
            _Packet.AddVInt(this.Level);
            _Packet.AddVInt(this.Score);
            _Packet.AddVInt(this.Donations);

            _Packet.AddVInt(0);

            _Packet.AddByte(this.CurrentRank);
            _Packet.AddByte(this.PreviousRank);
            _Packet.AddVInt(this.CrownChestCrowns);

            _Packet.AddVInt(0);
            _Packet.AddVInt(0);
            _Packet.AddVInt(0);

            _Packet.AddByte(0);
            _Packet.AddByte(0);

            _Packet.AddLong(this.Sender_ID);

            return _Packet.ToArray();
        }
    }
}
