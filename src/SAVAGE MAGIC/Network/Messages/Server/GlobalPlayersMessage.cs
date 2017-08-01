using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class GlobalPlayersMessage : Message
    {
        public GlobalPlayersMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 24403;
        }

        public override void Encode()
        {
            var data = new List<byte>();
            var packet1 = new List<byte>();
            var i = 0;

            foreach (var player in ResourcesManager.GetInMemoryLevels().OrderByDescending(t => t.Avatar.GetScore()))
            {
                if (player.Avatar.GetAvatarLevel() >= 70)
                {
                    try
                    {
                        var pl = player.Avatar;

                        if (i < 100)
                        {
                            packet1.AddInt64(pl.Id);
                            packet1.AddString(pl.GetAvatarName());
                            packet1.AddInt32(i + 1);
                            packet1.AddInt32(pl.GetScore());
                            packet1.AddInt32(i + 1);
                            packet1.AddInt32(pl.GetAvatarLevel());
                            packet1.AddInt32(100);
                            packet1.AddInt32(i);
                            packet1.AddInt32(100);
                            packet1.AddInt32(1);
                            packet1.AddInt32(pl.GetLeagueId());
                            packet1.AddString(pl.GetUserRegion().ToUpper());
                            packet1.AddInt64(pl.Id);
                            packet1.AddInt32(1);
                            packet1.AddInt32(1);
                            if (pl.GetAllianceId() > 0)
                            {
                                packet1.Add(1); // 1 = Have an alliance | 0 = No alliance
                                packet1.AddInt64(pl.GetAllianceId());
                                packet1.AddString(ObjectManager.GetAlliance(pl.GetAllianceId()).AllianceName);
                                packet1.AddInt32(ObjectManager.GetAlliance(pl.GetAllianceId()).AllianceBadgeData);
                            }
                            else
                                packet1.Add(0);
                            i++;
                        }
                        else
                            break;
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            data.AddInt32(i);
            data.AddRange(packet1);
            data.AddInt32(i);
            data.AddRange(packet1);

            data.AddInt32((int)TimeSpan.FromDays(7).TotalSeconds);
            data.AddInt32(DateTime.Now.Year);
            data.AddInt32(DateTime.Now.Month);
            data.AddInt32(DateTime.Now.Year);
            data.AddInt32(DateTime.Now.Month - 1);
            Encrypt(data.ToArray());
        }
    }
}
