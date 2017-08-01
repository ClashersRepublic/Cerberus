
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.Packets.Messages.Client
{
    internal class RevengeAttackerMessage : Message
    {
        public RevengeAttackerMessage(ClashOfClans.Client client, PacketReader br) : base(client, br)
        {
            // Space
        }

        public override void Decode()
        {
            // 5 int32, no need reading(for now).
        }

        public override void Process(Level level)
        {
            new OwnHomeDataMessage(Client, Client.Level).Send();
        }
    }
}
