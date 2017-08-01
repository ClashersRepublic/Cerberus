using System.Collections.Generic;

namespace Savage.Magic.Network.Messages.Server
{
    internal class AllianceJoinOkMessage : Message
    {
        public AllianceJoinOkMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 24303;
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            Encrypt(pack.ToArray());
        }
    }
}