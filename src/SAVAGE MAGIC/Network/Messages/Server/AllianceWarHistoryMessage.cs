using System;
using System.Collections.Generic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    // Packet 24338
    internal class AllianceWarHistoryMessage : Message
    {
        readonly Alliance m_vHomeAlliance;
        public AllianceWarHistoryMessage(ClashOfClans.Client client, Alliance home) : base(client)
        {
            MessageType = 24338;
            m_vHomeAlliance = home;
        }

        public override void Encode()
        {
            var data = new List<byte>();

            data.AddInt32(1);

            data.AddInt64(m_vHomeAlliance.AllianceId); // 1 Alliance ID
            data.AddString(m_vHomeAlliance.AllianceName); // 1 Alliance Name
            data.AddInt32(m_vHomeAlliance.AllianceBadgeData); // 1 Alliance Badge
            data.AddInt32(m_vHomeAlliance.AllianceLevel); // 1 Alliance Level

            data.AddInt64(9999); // 2 Alliance ID
            data.AddString("Goblinland"); // 2 Alliance Name
            data.AddInt32(0); // 2 Alliance Badge
            data.AddInt32(1); // 2 Alliance Level

            data.AddInt32(9999); // 1 Stars
            data.AddInt32(0); // 2 Stars

            data.AddInt32(0); // 1 Village Destroyed
            data.AddInt32(100); // 2 Village Destroyed

            data.AddInt32(0); // 1 Unknown
            data.AddInt32(0); // 2 Unknown

            data.AddInt32(100); // Attack Used
            data.AddInt32(4000); // XP Earned

            data.AddInt64(515631654); // War ID
            data.AddInt64(40); // Members Count

            data.AddInt32(1); // War Won Count

            data.Add((byte) 99);
            data.AddInt32((int) TimeSpan.FromDays(1).TotalSeconds);
            data.AddInt64((int) (TimeSpan.FromDays(1).TotalSeconds - TimeSpan.FromDays(0.5).TotalSeconds));

            Encrypt(data.ToArray());
        }
    }
}
