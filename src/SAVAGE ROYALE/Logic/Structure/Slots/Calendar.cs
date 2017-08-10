using System.Collections.Generic;
using CRepublic.Royale.Logic.Structure.Slots.Items;
using Newtonsoft.Json;

namespace CRepublic.Royale.Logic.Structure.Slots
{
    internal class Calendar
    {
        [JsonProperty("events")]  internal List<Event> Events = new List<Event>();
    }
}
