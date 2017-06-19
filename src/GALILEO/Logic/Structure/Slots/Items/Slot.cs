using Newtonsoft.Json;

namespace Republic.Magic.Logic.Structure.Slots.Items
{
    internal class Slot
    {
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Include)] internal int Data;
        [JsonProperty("cnt", DefaultValueHandling = DefaultValueHandling.Include)] internal int Count;

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