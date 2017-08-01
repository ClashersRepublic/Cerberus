using System.Collections.Generic;
using Savage.Magic;
using Savage.Magic.Logic.StreamEntries;

namespace Savage.Magic.Network.Messages.Server
{
    internal class AvatarChatBanMessage : Message
    {
        public int m_vCode = 86400;

        public AvatarChatBanMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 20118;
        }

        public override void Encode()
        {
            var data = new List<byte>();
            data.AddInt32(m_vCode);

            Encrypt(data.ToArray());
        }

        public void SetBanPeriod(int code)
        {
            m_vCode = code;
        }
    }
}