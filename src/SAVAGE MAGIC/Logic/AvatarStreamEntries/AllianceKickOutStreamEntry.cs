using System.Collections.Generic;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Logic.AvatarStreamEntries
{
    internal class AllianceKickOutStreamEntry : AvatarStreamEntry
    {
        private int m_vAllianceBadgeData;
        private long m_vAllianceId;
        private string m_vAllianceName;
        private string m_vMessage;

        public override byte[] Encode()
        {
            var data = new List<byte>();

            data.AddRange(base.Encode());
            data.AddString(m_vMessage);
            data.AddInt64(m_vAllianceId);
            data.AddString(m_vAllianceName);
            data.AddInt32(m_vAllianceBadgeData);
            data.Add((byte) 1);
            data.AddInt32(0x29);
            data.AddInt32(0x0084E879);

            return data.ToArray();
        }

        public override int GetStreamEntryType()
        {
            return 5;
        }

        public void SetAllianceBadgeData(int data)
        {
            m_vAllianceBadgeData = data;
        }

        public void SetAllianceId(long id)
        {
            m_vAllianceId = id;
        }

        public void SetAllianceName(string name)
        {
            m_vAllianceName = name;
        }

        public void SetMessage(string message)
        {
            m_vMessage = message;
        }
    }
}