using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BL.Servers.CoC.Logic.Structure.Slots.Items
{
    internal class Event
    {
        [JsonProperty("id")] internal int ID;

        [JsonProperty("version")] internal int Version;

        [JsonProperty("visibleTime")] internal string VisibleTime = DateTime.UtcNow.ToString();

        [JsonProperty("startTime")] internal string StarTime = DateTime.UtcNow.ToString();

        [JsonProperty("endTime")] internal string EndTime = DateTime.UtcNow.ToString();

        [JsonProperty("boomboxEntry")] internal string BoomBoxEntry = string.Empty;

        [JsonProperty("eventEntryName")] internal string EnventEntryName = string.Empty;

        [JsonProperty("inboxEntryId")] internal int InboxEntryID;

        [JsonProperty("notification")] internal string Notification = string.Empty;

        [JsonProperty("functions")] internal List<Functions> Functions = new List<Functions>();
    }
}
