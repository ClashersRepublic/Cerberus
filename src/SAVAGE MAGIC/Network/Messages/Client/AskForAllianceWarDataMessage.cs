using System.IO;

using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
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