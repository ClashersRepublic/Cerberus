using System.Collections.Generic;
using Magic.ClashOfClans.Logic.StreamEntries;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    // Packet 24312
    internal class AllianceStreamEntryMessage : Message
    {
        StreamEntry m_vStreamEntry;

        public AllianceStreamEntryMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 24312;
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddRange(m_vStreamEntry.Encode());
            Encrypt(pack.ToArray());
        }

        public void SetStreamEntry(StreamEntry entry)
        {
            m_vStreamEntry = entry;
        }
    }
}