using System.Collections.Generic;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    // Packet 24344
    internal class BookmarkRemoveAllianceMessage : Message
    {
        public BookmarkRemoveAllianceMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 24344;
        }

        public override void Encode()
        {
            var data = new List<byte>();
            data.Add(1);
            Encrypt(data.ToArray());
        }
    }
}