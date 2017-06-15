using System;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Logic.Enums;
using Newtonsoft.Json;

namespace BL.Servers.CoC.Logic.Structure.Slots.Items
{
    internal class Member
    {
        [JsonProperty("user_id")] internal long UserID;

        [JsonProperty("donations")] internal int Donations;
        [JsonProperty("received")] internal int Received;
        [JsonProperty("role")] internal Role Role = Role.Member;

        [JsonProperty("joined")] internal DateTime Joined = DateTime.UtcNow;

        internal bool Connected => Core.Resources.Players.ContainsKey(this.UserID);
        internal Level Player => Core.Resources.Players.Get(this.UserID, Constants.Database, false);
        internal bool New => this.Joined >= DateTime.UtcNow.AddDays(-1);

        internal Member()
        {
        }

        internal Member(Player Player)
        {
            this.UserID = Player.UserId;

            this.Joined = DateTime.UtcNow;
            this.Role = Role.Member;       
        }
    }
}
