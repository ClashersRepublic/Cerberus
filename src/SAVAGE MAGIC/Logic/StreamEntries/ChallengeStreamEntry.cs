using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Network;

namespace Magic.ClashOfClans.Logic.StreamEntries
{
    internal class ChallengeStreamEntry : Magic.ClashOfClans.Logic.StreamEntries.StreamEntry
    {
        private long m_vAvatarId;
        private string m_vAvatarName;

        public override byte[] Encode()
        {
            var data = new List<byte>();
            data.AddRange((IEnumerable<byte>) base.Encode());
            data.AddString(m_vMessage);
            data.AddInt32(0);
            return data.ToArray();
        }

        public override int GetStreamEntryType() => 12;

        public override void Load(JObject jsonObject)
        {
            m_vMessage = jsonObject["message"].ToObject<string>();
        }

        public override JObject Save(JObject jsonObject)
        {
            jsonObject = base.Save(jsonObject);
            jsonObject.Add("message", m_vMessage);
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
    }
}
