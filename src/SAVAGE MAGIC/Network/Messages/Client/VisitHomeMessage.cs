using System;
using System.IO;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class VisitHomeMessage : Message
    {
        internal long AvatarId;

        public VisitHomeMessage(ClashOfClans.Client client, PacketReader br)
            : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(Data)))
            {
                AvatarId = br.ReadInt64();
            }
        }

        public override void Process(Level level)
        {
            var player = ResourcesManager.GetPlayer(AvatarId, false);

            if (AvatarId == player.Avatar.Id)
            {
                player.Tick();

                new VisitedHomeDataMessage(Client, player, level).Send();

                if (level.Avatar.GetAllianceId() <= 0L)
                    return;

                Alliance alliance = ObjectManager.GetAlliance(level.Avatar.GetAllianceId());
                if (alliance == null)
                    return;

                new AllianceStreamMessage(Client, alliance).Send();
            }
            else
            {
                Logger.Error("ResourcesManager.GetPlayer returned a player with the wrong ID.");
            }
        }
    }
}
