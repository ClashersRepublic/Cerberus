using Newtonsoft.Json;

namespace BL.Servers.CoC.Logic.Structure.Slots.Items
{
    internal class Base
    {
        [JsonProperty("t", DefaultValueHandling = DefaultValueHandling.Include)] internal int Tick;
    }
}
