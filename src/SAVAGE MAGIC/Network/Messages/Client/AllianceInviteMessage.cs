using System.IO;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class AllianceInviteMessage : Message
    {
        public AllianceInviteMessage(ClashOfClans.Client client, PacketReader reader) : base(client, reader)
        {

        }
    }
}