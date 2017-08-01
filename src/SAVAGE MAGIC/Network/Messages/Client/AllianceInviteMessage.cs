using System.IO;
using Savage.Magic;
using Savage.Magic.Logic;

namespace Savage.Magic.Network.Messages.Client
{
    internal class AllianceInviteMessage : Message
    {
        public AllianceInviteMessage(ClashOfClans.Client client, PacketReader reader) : base(client, reader)
        {

        }
    }
}