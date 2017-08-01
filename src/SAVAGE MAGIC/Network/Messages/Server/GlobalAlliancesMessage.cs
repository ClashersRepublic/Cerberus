using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class GlobalAlliancesMessage : Message
    {
        List<Alliance> m_vAlliances;

        public GlobalAlliancesMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 24401;
        }

        public override void Encode()
        {
            var data = new List<byte>();
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

            data.AddInt32(i);
            data.AddRange((IEnumerable<byte>)packet1);

            data.AddInt32((int)TimeSpan.FromDays(1).TotalSeconds);
            data.AddInt32(3);
            data.AddInt32(50000);
            data.AddInt32(30000);
            data.AddInt32(15000);
            Encrypt(data.ToArray());
        }
    }
}
