using System.Collections.Generic;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Logic.AvatarStreamEntries
{
    internal class AllianceMailStreamEntry : AvatarStreamEntry
    {
        private int m_vAllianceBadgeData;
        private long m_vAllianceId;
        private string m_vAllianceName;
        private string m_vMessage;
        private long m_vSenderId;

        public override byte[] Encode()
        {
            var data = new List<byte>();
            data.AddRange((IEnumerable<byte>) base.Encode());
            data.AddString(m_vMessage);
            data.Add((byte) 1);
            data.AddInt64(m_vSenderId);
            data.AddInt64(m_vAllianceId);
            data.AddString(m_vAllianceName);
            data.AddInt32(m_vAllianceBadgeData);
            return data.ToArray();
        }

        public string GetMessage()
        {
            return this.m_vMessage;
        }

        public override int GetStreamEntryType()
        {
            return 6;
        }

        public void SetAllianceBadgeData(int data)
        {
            this.m_vAllianceBadgeData = data;
        }

        public void SetAllianceId(long id)
        {
            this.m_vAllianceId = id;
        }

        public void SetAllianceName(string name)
        {
            this.m_vAllianceName = name;
        }

        public void SetMessage(string message)
        {
            this.m_vMessage = message;
        }

        public void SetSenderId(long id)
        {
            this.m_vSenderId = id;
        }
    }
}
