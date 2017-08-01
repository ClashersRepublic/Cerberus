using System.Collections.Generic;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class ShutdownStartedMessage : Message
    {
        internal int m_vCode;

        public ShutdownStartedMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 20161;
        }

        public override void Encode()
        {
            var data = new List<byte>();
            data.AddInt32(m_vCode);
            Encrypt(data.ToArray());
        }

        public void SetCode(int code)
        {
            m_vCode = code;
        }
    }
}
