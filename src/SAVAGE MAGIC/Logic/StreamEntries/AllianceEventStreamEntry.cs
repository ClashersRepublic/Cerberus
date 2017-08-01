using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Logic.StreamEntries
{
    internal class AllianceEventStreamEntry : StreamEntry
    {
        private long m_vAvatarId;
        private string m_vAvatarName;
        private int m_vEventType;

        public override byte[] Encode()
        {
            var data = new List<byte>();
            data.AddRange((IEnumerable<byte>) base.Encode());
            data.AddInt32(m_vEventType);
            data.AddInt64(m_vAvatarId);
            data.AddString(m_vAvatarName);
            return data.ToArray();
        }

        public override int GetStreamEntryType() => 4;

        public override void Load(JObject jsonObject)
        {
            base.Load(jsonObject);
            m_vAvatarName = jsonObject["avatar_name"].ToObject<string>();
            m_vEventType = jsonObject["event_type"].ToObject<int>();
            m_vAvatarId = jsonObject["avatar_id"].ToObject<long>();
        }

        public override JObject Save(JObject jsonObject)
        {
            jsonObject = base.Save(jsonObject);
            jsonObject.Add("avatar_name", m_vAvatarName);
            jsonObject.Add("event_type", m_vEventType);
            jsonObject.Add("avatar_id", m_vAvatarId);
            return jsonObject;
        }

        public void SetAvatarId(long id)
        {
            m_vAvatarId = id;
        }

        public void SetAvatarName(string name)
        {
            m_vAvatarName = name;
        }

        public void SetEventType(int type)
        {
            m_vEventType = type;
        }
    }
}
