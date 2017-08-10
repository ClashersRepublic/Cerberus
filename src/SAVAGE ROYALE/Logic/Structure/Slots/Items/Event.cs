using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CRepublic.Royale.Logic.Structure.Slots.Items
{
    internal class Event
    {
        [JsonProperty("id")] internal int ID;

        [JsonProperty("name")] internal string Name = string.Empty;

        [JsonProperty("boomboxEntry")] internal string BoomBoxEntry = string.Empty;

        [JsonProperty("eventEntryName")] internal string EventEntryName = string.Empty;

        [JsonProperty("inboxEntryId")] internal int InboxEntryID = 0;

        [JsonProperty("sc", DefaultValueHandling = DefaultValueHandling.Ignore)] internal string SCFile = string.Empty;

        [JsonProperty("image", DefaultValueHandling = DefaultValueHandling.Ignore)] internal string Image = string.Empty;

        [JsonProperty("localization", DefaultValueHandling = DefaultValueHandling.Ignore)] internal string Localization = string.Empty;

        [JsonProperty("notification", DefaultValueHandling = DefaultValueHandling.Ignore)] internal string Notification = string.Empty;

        [JsonProperty("visibleTime")] internal string VisibleTime = DateTime.UtcNow.ToString();

        [JsonProperty("startTime")] internal string StarTime = DateTime.UtcNow.ToString();

        [JsonProperty("endTime")] internal string EndTime = DateTime.UtcNow.ToString();

        [JsonProperty("showUpcoming")] internal int ShowUpcoming = 1;

        [JsonProperty("functions")] internal List<Functions> Functions = new List<Functions>();
    }
}
