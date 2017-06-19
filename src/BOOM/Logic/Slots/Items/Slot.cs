namespace CRepublic.Boom.Logic.Slots.Items
{
    using Newtonsoft.Json;
    internal class Slot
    {
        [JsonProperty("data")] internal int Data;
        [JsonProperty("cnt")] internal int Count;
        [JsonProperty("t", DefaultValueHandling = DefaultValueHandling.Ignore)] internal bool Train;

        internal Slot()
        {

        }

        internal Slot(int Data, int Cnt, bool Train = false)
        {
            this.Data = Data;
            this.Count = Cnt;
            this.Train = Train;
        }
    }
}