using System.Collections.Generic;
using BL.Servers.CoC.Logic.Structure.Slots.Items;
using Newtonsoft.Json;

namespace BL.Servers.CoC.Logic.Structure.Slots
{
    internal class Calendar
    {
        [JsonProperty("events")]  internal List<Event> Events = new List<Event>();
    }
}
