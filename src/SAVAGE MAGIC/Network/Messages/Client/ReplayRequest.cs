using System.IO;

using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class ReplayRequestMessage : Message
    {
        public ReplayRequestMessage(ClashOfClans.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
        }

        public override void Process(Level level)
        {
            //new ReplayData(Client).Send();
            new OwnHomeDataMessage(Client, Client.Level).Send();
        }
    }
}