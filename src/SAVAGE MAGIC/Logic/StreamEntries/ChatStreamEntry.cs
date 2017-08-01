using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Logic.StreamEntries
{
    internal class ChatStreamEntry : StreamEntry
    {
        private new string m_vMessage;

        public override byte[] Encode()
        {
            var data = new List<byte>();
            data.AddRange(base.Encode());
            data.AddString(m_vMessage);
            return data.ToArray();
        }

        public string GetMessage() => m_vMessage;

        public override int GetStreamEntryType() => 2;

        public override void Load(JObject jsonObject)
        {
            base.Load(jsonObject);
            m_vMessage = jsonObject["message"].ToObject<string>();
        }

        public override JObject Save(JObject jsonObject)
        {
            jsonObject = base.Save(jsonObject);
            jsonObject.Add("message", m_vMessage);
            return jsonObject;
        }

        public void SetMessage(string message)
        {
            m_vMessage = message;
        }
    }
}
