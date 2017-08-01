using System;
using System.IO;
using Savage.Magic.Core;

using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.Messages.Client
{
    internal class AskForAllianceDataMessage : Message
    {
        private long m_vAllianceId;

        public AskForAllianceDataMessage(ClashOfClans.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
        }

        public override void Decode()
        {
            m_vAllianceId = Reader.ReadInt64();
        }

        public override void Process(Level level)
        {
            var alliance = ObjectManager.GetAlliance(m_vAllianceId);
            if (alliance == null)
                return;

            new AllianceDataMessage(Client, alliance).Send();
        }
    }
}
