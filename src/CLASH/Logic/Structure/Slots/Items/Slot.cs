using Newtonsoft.Json;

namespace BL.Servers.CoC.Logic.Structure.Slots.Items
{
    internal class Slot
    {
        [JsonProperty("data")] internal int Data;
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