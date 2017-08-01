using Magic.ClashOfClans.Core;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    // Packet 25892
    internal class DisconnectedMessage : Message
    {
		public DisconnectedMessage(ClashOfClans.Client client) : base(client)
		{
            // Space
		}
	}
}
