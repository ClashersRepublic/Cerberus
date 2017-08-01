using System;
using System.Collections.Generic;
using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Network;

namespace Savage.Magic.Network.Commands.Server
{
    internal class AllianceSettingChangedCommand : Command
    {
        private Alliance m_vAlliance;
        private Level m_vPlayer;

        public AllianceSettingChangedCommand()
        {
        }

        public override byte[] Encode()
        {
            m_vPlayer.Tick();
            List<byte> data = new List<byte>();
            data.AddInt64(m_vAlliance.AllianceId);
            data.AddInt32(m_vAlliance.AllianceBadgeData);
            data.AddInt32(m_vAlliance.AllianceLevel);
            data.AddInt32(0);
            return data.ToArray();
        }

        public void SetAlliance(Alliance alliance)
        {
            m_vAlliance = alliance;
        }

        public void SetPlayer(Level player)
        {
            m_vPlayer = player;
        }
    }
}