using System.IO;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    // Packet 14402
    internal class TopLocalAlliancesMessage : Message
    {
        public TopLocalAlliancesMessage(ClashOfClans.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
        }

        public override void Process(Level level)
        {              
              new LocalAlliancesMessage(Client).Send();
        }
    }
}
