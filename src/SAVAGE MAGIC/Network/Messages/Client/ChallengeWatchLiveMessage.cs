using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Network;
using Savage.Magic.Network.Messages.Server;

namespace Magic.Packets.Messages.Client
{
    internal class ChallengeWatchLiveMessage : Message
    {
        public ChallengeWatchLiveMessage(ClashOfClans.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            // Space
        }

        public override void Process(Level level)
        {
            new OwnHomeDataMessage(Client, level).Send();
        }
    }
}
