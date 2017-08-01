using System.IO;

using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.Messages.Client
{
    // Packet 14401
    internal class TopGlobalAlliancesMessage : Message
    {
        public int unknown;

        public TopGlobalAlliancesMessage(ClashOfClans.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
        }

        public override void Decode()
        {
            unknown = Data.Length == 10 ? Data[9] : Reader.Read();
        }

        public override void Process(Level level)
        {
            if (unknown == 0)
                new GlobalAlliancesMessage(Client).Send();
            else
                new LocalAlliancesMessage(Client).Send();
        }
    }
}
