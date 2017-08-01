using System;
using System.IO;
using System.Text;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Client
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