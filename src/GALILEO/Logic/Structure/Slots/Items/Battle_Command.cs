using Newtonsoft.Json;

namespace CRepublic.Magic.Logic.Structure.Slots.Items
{
    internal class Battle_Command
    {
        [JsonProperty("ct", DefaultValueHandling = DefaultValueHandling.Include)] internal int Command_Type;

        [JsonProperty("c", DefaultValueHandling = DefaultValueHandling.Include)] internal Command_Base Command_Base = new Command_Base();
    }
}