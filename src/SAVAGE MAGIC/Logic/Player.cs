using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CRepublic.Magic.Logic
{
    internal class Player 
    {
        [JsonProperty("avatar_id_high")] internal int UserHighId;
        [JsonProperty("avatar_id_low")] internal int UserLowId;

        [JsonProperty("alliance_id_high")] internal int ClanHighID;
        [JsonProperty("alliance_id_low")] internal int ClanLowID;

        [JsonProperty("token")] internal string Token;
        [JsonProperty("password")] internal string Password;

        [JsonProperty("name")] internal string Name = "NoNameYet";
    }
}
