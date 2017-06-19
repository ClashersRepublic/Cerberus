using System;
using CRepublic.Royale.Core;
using CRepublic.Royale.Logic.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using CRepublic.Royale.Extensions.List;

namespace CRepublic.Royale.Logic.Slots.Items
{
    class Member
    {
        [JsonProperty("user_id")] internal long UserID;

        [JsonProperty("donations")] internal int Donations;
        [JsonProperty("received")] internal int Received;
        [JsonProperty("role")] internal Alliance_Role Role = Alliance_Role.Member;

        [JsonProperty("joined")] internal DateTime Joined = DateTime.UtcNow;

        internal bool Connected => Core.Server_Resources.Players.ContainsKey(this.UserID);
        internal Player Player => Core.Server_Resources.Players.Get(this.UserID, Constants.Database, false);
        internal bool New => this.Joined >= DateTime.UtcNow.AddDays(-1);

        internal Clan Clan;


        internal Member()
        {
        }

        internal Member(Player Player)
        {
            this.UserID = Player.UserId;

            this.Joined = DateTime.UtcNow;
            this.Role = Alliance_Role.Member;

            this.Clan = Core.Server_Resources.Clans.Get(Player.ClanId);
        }


        internal byte[] ToBytes
        {
            get
            {
                List<byte> _Packet = new List<byte>();

                _Packet.AddLong(this.Player.UserId);
                _Packet.AddString(this.Player.Facebook.Identifier);
                _Packet.AddString(this.Player.Username);
                _Packet.AddSCID(this.Player.Arena);
                _Packet.AddVInt((int)this.Clan.Members[this.Player.UserId].Role);
                _Packet.AddVInt(this.Player.Level);
                _Packet.AddVInt(this.Player.Trophies);
                _Packet.AddVInt(this.Clan.Members[this.Player.UserId].Donations);
                _Packet.AddInt(0);
                _Packet.Add(1);
                _Packet.Add(2);
                _Packet.AddVInt(0);
                _Packet.AddVInt(0);
                _Packet.AddVInt(0);
                _Packet.Add(0);
                _Packet.Add(0);
                _Packet.AddLong(this.Player.UserId);

                return _Packet.ToArray();
            }
        }
    }
}
