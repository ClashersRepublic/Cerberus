using System.Collections.Generic;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    // Packet 24317
    internal class AnswerJoinRequestAllianceMessage : Message
    {
        public AnswerJoinRequestAllianceMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 24317;
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            Encrypt(pack.ToArray());
        }
    }
}