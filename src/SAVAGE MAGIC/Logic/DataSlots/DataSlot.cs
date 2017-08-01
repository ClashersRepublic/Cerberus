using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using Magic.ClashOfClans.Core;
using Magic.Files.Logic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Network;

namespace Magic.ClashOfClans.Logic
{
    internal class DataSlot
    {
        public Data Data;
        public int Value;

        public DataSlot(Data d, int value)
        {
            Data = d;
            Value = value;
        }

        public void Decode(PacketReader br)
        {
            Data = br.ReadDataReference();
            Value = br.ReadInt32();
        }

        public byte[] Encode()
        {
            var data = new List<byte>();
            data.AddInt32(Data.GetGlobalID());
            data.AddInt32(Value);
            return data.ToArray();
        }

        public void Load(JObject jsonObject)
        {
            Data = CsvManager.DataTables.GetDataById(jsonObject["global_id"].ToObject<int>());
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
