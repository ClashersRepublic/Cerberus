using Newtonsoft.Json;

namespace Republic.Magic.Logic.Structure.Slots.Items
{
    internal class Functions
    {
        [JsonProperty("name")] internal string Name = string.Empty;

        [JsonProperty("parameters")] internal int[] Parameters;
    }
}
