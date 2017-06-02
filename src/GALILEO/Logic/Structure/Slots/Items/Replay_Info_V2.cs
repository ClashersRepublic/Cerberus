using System.Collections.Generic;
using Newtonsoft.Json;

namespace BL.Servers.CoC.Logic.Structure.Slots.Items
{
    internal class Replay_Info_V2
    {
        internal JsonSerializerSettings Client_JsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None,
        };

        [JsonProperty("loot")] internal List<int[]> Loot = new List<int[]>();

        [JsonProperty("availableLoot")] internal List<int[]> Available_Loot = new List<int[]>();

        [JsonProperty("units")] internal List<int[]> Units = new List<int[]>();

        [JsonProperty("levels")] internal List<int[]> Levels = new List<int[]>();
        
        [JsonProperty("stats")] internal Replay_Stats Stats = new Replay_Stats();

        internal void Add_Unit(int Data, int Count)
        {
            int Index = this.Units.FindIndex(U => U[0] == Data);

            if (Index > -1)
                this.Units[Index][1] += Count;
            else
                this.Units.Add(new[] {Data, Count});
        }

        internal void Add_Level(int Data, int Count)
        {
            int Index = this.Levels.FindIndex(U => U[0] == Data);

            if (Index > -1)
                this.Levels[Index][1] += Count;
            else
                this.Levels.Add(new[] {Data, Count});
        }

        internal void Add_Available_Loot(int Data, int Count)
        {
            int Index = this.Available_Loot.FindIndex(U => U[0] == Data);

            if (Index > -1)
                this.Available_Loot[Index][1] += Count;
            else
                this.Available_Loot.Add(new[] {Data, Count});
        }

        internal string Json => JsonConvert.SerializeObject(new
        {
            loot = this.Loot,
            availableLoot = this.Available_Loot,
            units = this.Units,
            level = this.Levels,
            stats = this.Stats
        }, this.Client_JsonSettings);
    }
}
