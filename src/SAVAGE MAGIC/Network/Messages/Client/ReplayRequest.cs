using System.IO;

using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.Messages.Client
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