using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Logic.StreamEntries
{
    internal class InvitationStreamEntry : StreamEntry
    {
        public static string Message = "Hello, I would like to join your clan.";

        public override byte[] Encode()
        {
            var data = new List<byte>();
            data.AddRange((IEnumerable<byte>) base.Encode());
            data.AddString(Message);
            data.AddString(m_vJudge);
            data.AddInt32(m_vState);
            return data.ToArray();
        }

        public override int GetStreamEntryType() => 3;

        public override void Load(JObject jsonObject)
        {
            base.Load(jsonObject);
            Message = jsonObject["message"].ToObject<string>();
            m_vJudge = jsonObject["judge"].ToObject<string>();
            m_vState = jsonObject["state"].ToObject<int>();
        }

        public override JObject Save(JObject jsonObject)
        {
            jsonObject = base.Save(jsonObject);
            jsonObject.Add("message", Message);
            jsonObject.Add("judge", m_vJudge);
            jsonObject.Add("state", m_vState);
            return jsonObject;
        }

        public new void SetJudgeName(string name)
        {
            m_vJudge = name;
        }

        public new void SetMessage(string message)
        {
            Message = message;
        }

        public new void SetState(int status)
        {
            m_vState = status;
        }
    }
}