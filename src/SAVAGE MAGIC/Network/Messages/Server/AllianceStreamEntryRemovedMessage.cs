using System.Collections.Generic;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    // Packet 24318
    internal class AllianceStreamEntryRemovedMessage : Message
    {
        public AllianceStreamEntryRemovedMessage(ClashOfClans.Client client, int i) : base(client)
        {
            MessageType = 24318;
            m_vId = i;
        }

        public int m_vId;

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddInt32(0);
            pack.AddInt32(m_vId);
            Encrypt(pack.ToArray());
        }
    }
}
