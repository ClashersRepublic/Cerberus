using Newtonsoft.Json;

namespace Republic.Magic.Logic.Structure.Slots.Items
{
    internal class Command_Base
    {
        [JsonProperty("base", DefaultValueHandling = DefaultValueHandling.Include)] internal Base Base = new Base();

        [JsonProperty("d")] internal int Data;

        [JsonProperty("x")] internal int X;

        [JsonProperty("y")] internal int Y;

        [JsonProperty("t")] internal int Tick;
    }
}
