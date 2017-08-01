using System.Collections.Generic;
using Magic.ClashOfClans.Network.Messages.Client;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    // Packet 20108
    internal class KeepAliveOkMessage : Message
    {
        public KeepAliveOkMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 20108;
        }

        public override void Encode()
        {
            var data = new List<byte>();
            Encrypt(data.ToArray());
        }
    }
}