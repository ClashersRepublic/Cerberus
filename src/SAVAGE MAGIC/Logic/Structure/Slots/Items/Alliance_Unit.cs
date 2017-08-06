using Newtonsoft.Json;

namespace CRepublic.Magic.Logic.Structure.Slots.Items
{
    internal class Alliance_Unit
    {
        [JsonProperty("pl_id")] internal long Player_ID = 0;

        [JsonProperty("id")] internal int Data;

        [JsonProperty("cnt")] internal int Count;

        [JsonProperty("lvl")] internal int Level = 0;

        internal Alliance_Unit() { }

        internal Alliance_Unit(long _PlayerId, int _ID, int _Count, int _Level)
        {
            this.Player_ID = _PlayerId;
            this.Data = _ID;
            this.Count = _Count;
            this.Level = _Level;
        }
    }
}