using System;
using System.IO;

using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
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