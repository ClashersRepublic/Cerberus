using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    // Packet 24404
    internal class LocalPlayersMessage : Message
    {
        public LocalPlayersMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 24404;
        }

        public override void Encode()
        {
            var packet = new List<byte>();
            var data = new List<byte>();
            var i = 0;

            foreach(var player in ResourcesManager.GetInMemoryLevels().OrderByDescending(t => t.Avatar.GetScore()))
            {
                var pl = player.Avatar;
                var id = pl.GetAllianceId();
                if (i >= 100)
                    break;
                data.AddInt64(pl.Id);
                data.AddString(pl.GetAvatarName());
                data.AddInt32(i + 1);
                data.AddInt32(pl.GetScore());
                data.AddInt32(i + 1);
                data.AddInt32(pl.GetAvatarLevel());
                data.AddInt32(100);
                data.AddInt32(1);
                data.AddInt32(100);
                data.AddInt32(1);
                data.AddInt32(pl.GetLeagueId());
                data.AddString(pl.GetUserRegion().ToUpper());
                data.AddInt64(pl.GetAllianceId());
                data.AddInt32(1);
                data.AddInt32(1);
                if (pl.GetAllianceId() > 0)
                {
                    data.Add(1); // 1 = Have an alliance | 0 = No alliance
                    data.AddInt64(pl.GetAllianceId());
                    data.AddString(ObjectManager.GetAlliance(id).AllianceName);
                    data.AddInt32(ObjectManager.GetAlliance(id).AllianceBadgeData);
                }
                else
                    data.Add(0);
                i++;
            }

            packet.AddInt32(i);
            packet.AddRange(data.ToArray());
            Encrypt(packet.ToArray());
        }
    }
}
