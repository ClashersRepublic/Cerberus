using System.Collections.Generic;
using System.Threading.Tasks;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class AllianceFullEntryMessage : Message
    {
       public readonly Alliance m_vAlliance;

        public AllianceFullEntryMessage(ClashOfClans.Client client, Alliance alliance) : base(client)
        {
            MessageType = 24324;
            m_vAlliance = alliance;
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            var allianceMembers = m_vAlliance.AllianceMembers;
            pack.AddString(m_vAlliance.AllianceDescription);
            pack.AddInt32(0);
            pack.AddInt32(0);
            pack.Add((byte) 0);
            pack.Add((byte) 0);
            pack.AddInt32(0);
            pack.AddRange((IEnumerable<byte>) m_vAlliance.EncodeFullEntry());

            Encrypt(pack.ToArray());
        }
    }
}
