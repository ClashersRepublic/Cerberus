using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class AllianceStreamMessage : Message
    {
        private readonly Alliance m_vAlliance;

        public AllianceStreamMessage(ClashOfClans.Client client, Alliance alliance) : base(client)
        {
            MessageType = 24311;
            m_vAlliance = alliance;
        }

        public override void Encode()
        {
            System.Collections.Generic.List<Magic.ClashOfClans.Logic.StreamEntries.StreamEntry> list = this.m_vAlliance.ChatMessages.ToList<Magic.ClashOfClans.Logic.StreamEntries.StreamEntry>();
            var pack = new List<byte>();
            var chatMessages = m_vAlliance.ChatMessages.ToList();
            pack.AddInt32(0);
            pack.AddInt32(list.Count);
            int num = 0;

            foreach (Magic.ClashOfClans.Logic.StreamEntries.StreamEntry streamEntry in list)
            {
                pack.AddRange((IEnumerable<byte>)streamEntry.Encode());
                ++num;
                if (num >= 150)
                    break;
            }

            Encrypt(pack.ToArray());
        }
    }
}
