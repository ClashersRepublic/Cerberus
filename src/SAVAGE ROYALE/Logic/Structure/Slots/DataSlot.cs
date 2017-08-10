using CRepublic.Royale.Files.CSV_Helpers;
using CRepublic.Royale.Files;
using Newtonsoft.Json.Linq;

namespace CRepublic.Royale.Logic.Structure.Slots
{
    internal class DataSlot
    {
        internal Data Data;
        internal int Value;

        public DataSlot(Data d, int value)
        {
            this.Data = d;
            this.Value = value;
        }

        public void Load(JObject jsonObject)
        {
            this.Data = CSV.Tables.GetWithGlobalID(jsonObject["id"].ToObject<int>());
            this.Value = jsonObject["cnt"].ToObject<int>();
        }

        public JObject Save(JObject jsonObject)
        {
            jsonObject.Add("id", this.Data.GetGlobalID());
            jsonObject.Add("cnt", this.Value);
            return jsonObject;
        }

    }
}
