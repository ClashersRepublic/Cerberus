using System.Collections.Generic;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    // Packet 24111
    internal class AvailableServerCommandMessage : Message
    {
        public AvailableServerCommandMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 24111;
        }

        Command m_vCommand;
        int m_vServerCommandId;

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddInt32(m_vServerCommandId);
            pack.AddRange((IEnumerable<byte>) m_vCommand.Encode());
            Encrypt(pack.ToArray());
        }

        public void SetCommand(Command c)
        {
            m_vCommand = c;
        }

        public void SetCommandId(int id)
        {
            m_vServerCommandId = id;
        }
    }
}