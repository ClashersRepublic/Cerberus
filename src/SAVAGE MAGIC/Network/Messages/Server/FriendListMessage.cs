using System.Collections.Generic;
using Savage.Magic;
using Savage.Magic.Logic;

namespace Savage.Magic.Network.Messages.Server
{
    // Packet 20105
    internal class FriendListMessage : Message
    {
        public FriendListMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 20105;
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddInt32(0);
            pack.AddInt32(0);
            pack.AddDataSlots(new List<DataSlot>());
            Encrypt(pack.ToArray());
        }
    }
}