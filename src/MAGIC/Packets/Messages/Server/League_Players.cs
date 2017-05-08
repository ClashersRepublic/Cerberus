using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Core;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic;

namespace BL.Servers.CoC.Packets.Messages.Server
{

    internal class League_Players : Message
    {
        public League_Players(Device client) : base(client)
        {
            this.Identifier = 24503;
        }

        internal override void Encode()
        {
            List<byte> packet1 = new List<byte>();
            int i = 1;
            foreach (Level player in Resources.Players.Values
                .Where(t => t.Avatar.League == this.Device.Player.Avatar.League)
                .OrderByDescending(t => t.Avatar.Trophies))
            {
                if (i >= 51)
                    break;

                Player pl = player.Avatar;
                packet1.AddLong(pl.UserId);
                packet1.AddString(pl.Name);
                packet1.AddInt(i);
                packet1.AddInt(pl.Trophies);
                packet1.AddInt(i);
                packet1.AddInt(pl.Level);
                packet1.AddInt(200);
                packet1.AddInt(i);
                packet1.AddInt(100);
                packet1.AddInt(1);
                packet1.AddLong(pl.ClanId);
                packet1.AddInt(1);
                packet1.AddInt(1);
                if (pl.ClanId > 0)
                {
                    packet1.Add(1);
                    packet1.AddLong(pl.ClanId);
                    //Alliance _Alliance = ObjectManager.GetAlliance(pl.AllianceId);
                    //packet1.AddString(_Alliance.m_vAllianceName);
                    //packet1.AddInt(_Alliance.m_vAllianceBadgeData);
                    packet1.AddLong(i);
                }
                else
                    packet1.Add(0);
                i++;

            }
            this.Data.AddInt(9000); //Season End
            this.Data.AddInt(i - 1);
            this.Data.AddRange(packet1);
        }
    }
}
