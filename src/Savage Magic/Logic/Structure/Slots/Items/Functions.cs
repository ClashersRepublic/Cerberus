using Newtonsoft.Json;

namespace CRepublic.Magic.Logic.Structure.Slots.Items
{
    internal class Functions
    {
        [JsonProperty("name")] internal string Name = string.Empty;

        [JsonProperty("parameters")] internal string[] Parameters;
    }
}
