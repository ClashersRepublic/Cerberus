using System;
using System.IO;

using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.Messages.Client
{
    internal class KeepAliveMessage : Message
    {
        public KeepAliveMessage(ClashOfClans.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
        }

        public override void Process(Level level)
        {
            Client.LastKeepAlive = DateTime.Now;
            Client.NextKeepAlive = Client.LastKeepAlive.AddSeconds(30);

            Client._keepAliveOk.Send();
        }
    }
}