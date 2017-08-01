using System.IO;

using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.Messages.Client
{
    internal class AskForAllianceWarDataMessage : Message
    {
        public AskForAllianceWarDataMessage(ClashOfClans.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(Data)))
            {
            }
        }

        public override void Process(Level level)
        {
            new AllianceWarDataMessage(Client).Send();
        }
    }
}