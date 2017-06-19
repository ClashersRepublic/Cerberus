namespace CRepublic.Boom.Logic.Slots
{
    using System.Collections.Generic;
    using CRepublic.Boom.Extensions.Binary;
    using CRepublic.Boom.Extensions.List;
    using CRepublic.Boom.Files;
    using CRepublic.Boom.Files.CSV_Helpers;
    using Newtonsoft.Json.Linq;

    internal class DataSlot
    {
        public DataSlot(Data d, int value)
        {
            Data = d;
            Value = value;
        }

        public Data Data;
        public int Value;

        public void Decode(Reader br)
        {
            Data = br.ReadData();
            Value = br.ReadInt32();
        }

        public byte[] Encode()
        {
            List<byte> data = new List<byte>();
            data.AddInt(Data.GetGlobalID());
            data.AddInt(Value);
            return data.ToArray();
        }

        public void Load(JObject jsonObject)
        {
            Data = CSV.Tables.GetWithGlobalID(jsonObject["global_id"].ToObject<int>());
            Value = jsonObject["value"].ToObject<int>();
        }

        public JObject Save(JObject jsonObject)
        {
            jsonObject.Add("global_id", Data.GetGlobalID());
            jsonObject.Add("value", Value);
            return jsonObject;
        }
    }
}
