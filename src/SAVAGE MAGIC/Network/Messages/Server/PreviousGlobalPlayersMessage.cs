using System;
using System.Collections.Generic;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class PreviousGlobalPlayersMessage : Message
    {
        public PreviousGlobalPlayersMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 24405;
        }

        public override void Encode()
        {
            var data = new List<byte>();
            var packet1 = new List<byte>();

            var i = 1;
            foreach (var player in ResourcesManager.OnlinePlayers)
            {
                var pl = player.Avatar;
                packet1.AddInt64(pl.Id); 
                packet1.AddString(pl.GetAvatarName()); 
                packet1.AddInt32(i); 
                packet1.AddInt32(pl.GetScore()); 
                packet1.AddInt32(i); 
                packet1.AddInt32(pl.GetAvatarLevel());
                packet1.AddInt32(100); 
                packet1.AddInt32(1); 
                packet1.AddInt32(100); 
                packet1.AddInt32(1);
                packet1.AddInt32(pl.GetLeagueId()); 
                packet1.AddString("EN"); 
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
                if (i >= 101)
                    break;
                i++;
            }
            data.AddInt32(i - 1);
            data.AddRange(packet1);
            data.AddInt32(DateTime.Now.Month - 1);
            data.AddInt32(DateTime.Now.Year);
            Encrypt(data.ToArray());
        }
    }
}
