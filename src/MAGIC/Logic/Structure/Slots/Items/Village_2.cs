using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CRepublic.Magic.Logic.Structure.Slots.Items
{
    internal class Village_2
    {
        [JsonProperty("TownHallMaxLevel")] internal int TownHallMaxLevel = 5;
        [JsonProperty("ScoreChangeForLosing")] internal JArray ScoreChangeForLosing = new JArray();
        [JsonProperty("StrengthRangeForScore")] internal JArray StrengthRangeForScore = new JArray();

    }
}
