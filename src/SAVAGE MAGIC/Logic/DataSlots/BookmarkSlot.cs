using Savage.Magic.Network;
using Savage.Magic;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Savage.Magic.Logic.DataSlots
{
    internal class BookmarkSlot
    {
        public long Value;

        public BookmarkSlot(long value)
        {
            Value = value;
        }

        public void Decode(PacketReader br)
        {
            Value = (long) br.ReadInt32();
        }

        public byte[] Encode()
        {
            var data = new List<byte>();
            data.AddInt64(Value);
            return data.ToArray();
        }

        public void Load(JObject jsonObject)
        {
            Value = jsonObject["id"].ToObject<int>();
        }

        public JObject Save(JObject jsonObject)
        {
            jsonObject.Add("id", Value);
            return jsonObject;
        }
    }
}
