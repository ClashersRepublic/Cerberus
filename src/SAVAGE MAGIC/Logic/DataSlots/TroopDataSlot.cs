using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.ClashOfClans.Core;
using Magic.Files.Logic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Network;

namespace Magic.ClashOfClans.Logic.DataSlots
{
    internal class TroopDataSlot
    {
        public Data Data;
        public int Value;
        public int Value1;

        public TroopDataSlot(Data d, int value, int value1)
        {
            Data = d;
            Value = value;
            Value1 = value1;
        }

        public void Decode(PacketReader br)
        {
            Data = br.ReadDataReference();
            Value = br.ReadInt32();
            Value1 = br.ReadInt32();
        }

        public byte[] Encode()
        {
            var data = new List<byte>();
            data.AddInt32(Data.GetGlobalID());
            data.AddInt32(Value);
            data.AddInt32(Value1);
            return data.ToArray();
        }

        public void Load(JObject jsonObject)
        {
            Data = CsvManager.DataTables.GetDataById(jsonObject["global_id"].ToObject<int>());
            Value = jsonObject["count"].ToObject<int>();
            Value1 = jsonObject["level"].ToObject<int>();
        }

        public JObject Save(JObject jsonObject)
        {
            jsonObject.Add("global_id", Data.GetGlobalID());
            jsonObject.Add("count", Value);
            jsonObject.Add("level", Value1);
            return jsonObject;
        }
    }
}
