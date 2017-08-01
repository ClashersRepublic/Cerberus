using System.IO;
using Magic.ClashOfClans.Core;

using Magic.Files.Logic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
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
