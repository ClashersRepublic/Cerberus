using System.Collections.Generic;
using Republic.Magic.Logic.Structure.Slots.Items;
using Newtonsoft.Json;

namespace Republic.Magic.Logic.Structure.Slots
{
    internal class Calendar
    {
        [JsonProperty("events")]  internal List<Event> Events = new List<Event>();
    }
}
