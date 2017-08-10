using Newtonsoft.Json;

namespace CRepublic.Royale.Logic.Structure.Slots.Items
{
    internal class Slot
    {
        [JsonProperty("id")] internal int Data;
        [JsonProperty("cnt")] internal int Count;

        internal Slot()
        {

        }

        internal Slot(int Data, int Cnt)
        {
            this.Data = Data;
            this.Count = Cnt;
        }
    }
}