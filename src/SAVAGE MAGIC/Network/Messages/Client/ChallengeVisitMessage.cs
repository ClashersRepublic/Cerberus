using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Savage.Magic.Core;

using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Network;
using Savage.Magic.Network.Messages.Server;

namespace Magic.Packets.Messages.Client
{
    internal class ChallengeVisitMessage : Message
    {
        public long AvatarID;

        public ChallengeVisitMessage(ClashOfClans.Client client, PacketReader br) : base(client, br)
        {
            // Space
        }

        public override void Decode()
        {
            AvatarID = Reader.ReadInt64();
        }

        public override void Process(Level level)
        {
            new OwnHomeDataMessage(Client, level).Send();
        }
    }
}
