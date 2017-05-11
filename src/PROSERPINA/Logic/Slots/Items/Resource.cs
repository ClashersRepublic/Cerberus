using Newtonsoft.Json;

namespace BL.Servers.CR.Logic.Slots.Items
{
    internal class Resource
    {
        [JsonProperty("type")] internal int Type = 0x05;
        [JsonProperty("id")] internal int Identifier;
        [JsonProperty("value")] internal int Value;

        internal Resource()
        {
            // Resource
        }

        internal Resource(int Identifier, int Value)
        {
            this.Identifier = Identifier;
            this.Value = Value;
        }
    }
}