using Savage.Magic.Core;
using Savage.Magic;
using Savage.Magic.Logic;

namespace Savage.Magic.Network.Messages.Server
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
