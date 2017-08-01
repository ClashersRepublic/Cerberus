using System.Collections.Generic;
using Savage.Magic.Logic.AvatarStreamEntries;

namespace Savage.Magic.Network.Messages.Server
{
    // Packet 24412
    internal class AvatarStreamEntryMessage : Message
    {
        AvatarStreamEntry m_vAvatarStreamEntry;

        public AvatarStreamEntryMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 24412;
        }

        public override void Encode()
        {
            var pack = new List<byte>();   
            pack.AddRange(m_vAvatarStreamEntry.Encode());
            Encrypt(pack.ToArray());
        }

        public void SetAvatarStreamEntry(AvatarStreamEntry entry)
        {
            m_vAvatarStreamEntry = entry;
        }
    }
}