using System.Collections.Generic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class GlobalChatLineMessage : Message
    {
        internal readonly int m_vPlayerLevel;
        internal int m_vAllianceIcon;
        internal int m_vLeagueId;
        internal long m_vAllianceId;
        internal string m_vAllianceName;
        internal long m_vCurrentHomeId;
        internal bool m_vHasAlliance;
        internal long m_vHomeId;
        internal string m_vMessage;
        internal string m_vPlayerName;

        public GlobalChatLineMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 24715;

            m_vMessage = "default";
            m_vPlayerName = "default";
            m_vHomeId = 1;
            m_vCurrentHomeId = 1;
            m_vPlayerLevel = 1;
            m_vHasAlliance = false;
        }

        public override void Encode()
        {
            var data = new List<byte>();

            data.AddString(m_vMessage);
            data.AddString(m_vPlayerName);
            data.AddInt32(m_vPlayerLevel);
            data.AddInt32(m_vLeagueId);
            data.AddInt64(m_vHomeId);
            data.AddInt64(m_vCurrentHomeId);

            if (!m_vHasAlliance)
            {
                data.Add(0);
            }
            else
            {
                data.Add(1);
                data.AddInt64(m_vAllianceId);
                data.AddString(m_vAllianceName);
                data.AddInt32(m_vAllianceIcon);
            }

            Encrypt(data.ToArray());
        }

        public void SetAlliance(Alliance alliance)
        {
            if (alliance == null || alliance.AllianceId<= 0L)
            {
                // Just in case.
                m_vHasAlliance = false;
                return;
            }

            m_vHasAlliance = true;
            m_vAllianceId = alliance.AllianceId;
            m_vAllianceName = alliance.AllianceName;
            m_vAllianceIcon = alliance.AllianceBadgeData;
        }

        public void SetChatMessage(string message)
        {
            m_vMessage = message;
        }

        public void SetLeagueId(int leagueId)
        {
            m_vLeagueId = leagueId;
        }

        public void SetPlayerId(long id)
        {
            m_vHomeId = id;
            m_vCurrentHomeId = id;
        }

        public void SetPlayerName(string name)
        {
            m_vPlayerName = name;
        }
    }
}