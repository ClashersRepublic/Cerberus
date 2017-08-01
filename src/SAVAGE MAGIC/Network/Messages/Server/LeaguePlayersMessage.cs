using System;
using System.Collections.Generic;
using System.Linq;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    // Packet 24503
    internal class LeaguePlayersMessage : Message
    {
        public LeaguePlayersMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 24503;
            Player = client.Level;
        }

        public static Level Player { get; set; }

        public override void Encode()
        {
            var data = new List<byte>();
            var packet1 = new List<byte>();
            var i = 1;

            foreach (var player in ResourcesManager.OnlinePlayers.OrderByDescending(t => t.Avatar.GetScore()))
            {
                if (i < 51)
                {
                    Avatar avatar = player.Avatar;
                    if (player.Avatar.GetAvatarName() != null)
                    {
                        try
                        {
                            var pl = player.Avatar;
                            packet1.AddInt64(pl.Id);
                            packet1.AddString(pl.GetAvatarName());
                            packet1.AddInt32(i);
                            packet1.AddInt32(pl.GetScore());
                            packet1.AddInt32(i);
                            packet1.AddInt32(pl.GetAvatarLevel());
                            packet1.AddInt32(200);
                            packet1.AddInt32(i);
                            packet1.AddInt32(100);
                            packet1.AddInt32(1);
                            packet1.AddInt64(pl.GetAllianceId());
                            packet1.AddInt32(1);
                            packet1.AddInt32(1);
                            if (pl.GetAllianceId() > 0)
                            {
                                packet1.Add(1); // 1 = Have an alliance | 0 = No alliance
                                packet1.AddInt64(pl.GetAllianceId());
                                packet1.AddString(ObjectManager.GetAlliance(pl.GetAllianceId()).AllianceName);
                                packet1.AddInt32(ObjectManager.GetAlliance(pl.GetAllianceId()).AllianceBadgeData);
                                packet1.AddInt64(i);
                            }
                            else
                                packet1.Add(0);
                            i++;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                else
                    break;
            }
                data.AddInt32(9000); //Season End
            data.AddInt32(i - 1);
            data.AddRange(packet1);

            Encrypt(data.ToArray());
        }
    }
}