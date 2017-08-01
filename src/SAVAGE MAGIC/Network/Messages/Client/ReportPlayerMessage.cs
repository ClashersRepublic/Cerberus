using System;
using System.IO;
using Magic.ClashOfClans.Core;

using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class ReportPlayerMessage : Message
    {
        public long ReportedPlayerID { get; set; }

        public int Tick { get; set; }

        public ReportPlayerMessage(ClashOfClans.Client client, PacketReader br) : base(client, br)
        {

        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(Data)))
            {
                br.ReadInt32();
                ReportedPlayerID = br.ReadInt64();
                br.ReadInt32();
            }
        }

        public override void Process(Level level)
        {
            var player = ResourcesManager.GetPlayer(ReportedPlayerID, false);
            ++player.Avatar.ReportedTimes;
            if (player.Avatar.ReportedTimes < 3)
                return;

            AvatarChatBanMessage c = new AvatarChatBanMessage(Client);
            int code = 1800;
            c.SetBanPeriod(code);
            c.Send();
        }
    }
}
