using System.Collections.Generic;
using Republic.Magic.Logic.Structure.Slots.Items;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Republic.Magic.Logic.Structure.Slots
{
    internal class Globals_Replay
    {
        [JsonProperty("Village2")] internal Village_2 Village_2 = new Village_2();
        [JsonProperty("KillSwitches")] internal Kill_Switches KillSwitches = new Kill_Switches();
    }
}
