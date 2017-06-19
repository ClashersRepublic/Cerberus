using Newtonsoft.Json;

namespace CRepublic.Magic.Logic.Structure.Slots.Items
{
    internal class Base
    {
        [JsonProperty("t", DefaultValueHandling = DefaultValueHandling.Include)] internal int Tick;
    }
}
