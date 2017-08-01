using System.Collections.Generic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    // Packet 24111
    internal class LeaveAllianceOkMessage : Message
    {
        public LeaveAllianceOkMessage(ClashOfClans.Client client, Alliance alliance)
            : base(client)
        {
            MessageType = 24111;

            m_vServerCommandType = 0x02;
            m_vAlliance = alliance;
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddInt32(m_vServerCommandType);
            pack.AddInt64(m_vAlliance.AllianceId);
            pack.AddInt32(1); // 1 = leave, 2 = kick
            pack.AddInt32(-1);
            Encrypt(pack.ToArray());
        }

        internal Alliance m_vAlliance;
        internal int m_vServerCommandType;
    }
}