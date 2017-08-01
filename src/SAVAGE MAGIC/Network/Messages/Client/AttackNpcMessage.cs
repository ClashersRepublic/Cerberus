using System.IO;
using Savage.Magic.Core;

using Magic.Files.Logic;
using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.Messages.Client
{
    internal class AttackNpcMessage : Message
    {
        public int LevelId { get; set; }

        public AttackNpcMessage(ClashOfClans.Client client, PacketReader br)
            : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(Data)))
            {
                LevelId = br.ReadInt32();
            }
        }

        public override void Process(Level level)
        {
            new NpcDataMessage(Client, level, this).Send();
        }
    }
}
