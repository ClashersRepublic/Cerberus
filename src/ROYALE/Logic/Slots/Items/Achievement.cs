using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CRepublic.Royale.Logic.Slots.Items
{
    internal class Achievement
    {
        [JsonProperty("type")] internal int Type = 0x3C;
        [JsonProperty("id")] internal int Identifier;
        [JsonProperty("value")] internal int Value;

        internal Achievement()
        {
            // Achievement.
        }

        internal Achievement(int Identifier, int Value)
        {
            this.Identifier = Identifier;
            this.Value = Value;
        }
    }
}