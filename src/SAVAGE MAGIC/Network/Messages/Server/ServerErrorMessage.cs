using System.Collections.Generic;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class ServerErrorMessage : Message
    {
        internal string m_vErrorMessage;

        public ServerErrorMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 24115;
        }

        public override void Encode()
        {
            var data = new List<byte>();
            data.AddString(m_vErrorMessage);
            Encrypt(data.ToArray());
        }

        public void SetErrorMessage(string message)
        {
            m_vErrorMessage = message;
        }
    }
}
