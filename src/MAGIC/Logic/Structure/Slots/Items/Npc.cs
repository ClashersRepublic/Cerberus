using Newtonsoft.Json;

namespace BL.Servers.CoC.Logic.Structure.Slots.Items
{
    internal class Npc
    {
        [JsonProperty("npc_id")] internal int NPC_Id;

        [JsonProperty("star_gained")] internal int Star_Gained;
        [JsonProperty("gold_looted")] internal int Gold_Looted;
        [JsonProperty("elixir_looted")] internal int Elixir_Looted;
        [JsonProperty("dark_elixir_looted")] internal int Dark_Elixir_Looted;

        internal Npc()
        {

        }

        internal Npc(int NPC_Id, int Star = 3, int Gold = 1000000, int Elixir = 1000000, int DarkElixir = 1000000)
        {
            this.NPC_Id = NPC_Id;

            this.Star_Gained = Star;
            this.Gold_Looted = Gold;
            this.Elixir_Looted = Elixir;
            this.Dark_Elixir_Looted = DarkElixir;
        }
    }
}