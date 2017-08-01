using System.Collections.Generic;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    // Packet 24104
    internal class OutOfSyncMessage : Message
    {
        public OutOfSyncMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 24104;
        }

        public override void Encode()
        {
            var data = new List<byte>();
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(0);
            Encrypt(data.ToArray());
        }
    }
}
