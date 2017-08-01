using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    // Packet 24402
    internal class LocalAlliancesMessage : Message
    {
        List<Alliance> m_vAlliances;

        public LocalAlliancesMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 24402;
            m_vAlliances = new List<Alliance>();
        }

        public override void Encode()
        {
            var packet = new List<byte>();
            var packet1 = new List<byte>();
            var i = 0;

            foreach (var alliance in ObjectManager.GetInMemoryAlliances().OrderByDescending(t => t.Score))
            {
                if (i < 100)
                {
                    packet1.AddInt64(alliance.AllianceId);
                packet1.AddString(alliance.AllianceName);
                packet1.AddInt32(i + 1);
                packet1.AddInt32(alliance.Score);
                packet1.AddInt32(i + 1);
                packet1.AddInt32(alliance.AllianceBadgeData);
                packet1.AddInt32(alliance.AllianceMembers.Count);
                packet1.AddInt32(0);
                packet1.AddInt32(alliance.AllianceLevel);
                i++;
            }
            else
            break;
            }
            packet.AddInt32(0);       
            packet.AddRange(packet1.ToArray());
            Encrypt(packet.ToArray());
        }
    }
}
