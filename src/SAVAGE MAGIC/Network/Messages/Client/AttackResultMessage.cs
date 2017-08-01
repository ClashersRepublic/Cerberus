using System;
using System.IO;
using System.Text;
using Savage.Magic.Core;
using Savage.Magic;
using Savage.Magic.Logic;

namespace Savage.Magic.Network.Messages.Client
{
    internal class AttackResultMessage : Message
    {
        public AttackResultMessage(ClashOfClans.Client client, PacketReader br)
            : base(client, br)
        {

        }

        public override void Decode()
        {
        }

        public override void Process(Level level)
        {
        }
    }
}