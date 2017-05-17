using BL.Servers.CoC.Core;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CoC.Packets.Messages.Server.Leaderboard
{
    internal class Global_Clans : Message
    {
        internal List<Clan> Clans;

        public Global_Clans(Device Device) : base(Device)
        {
            this.Identifier = 24401;

            this.Clans = Resources.Clans.Values.OrderByDescending(t => t.Trophies).Take(100).ToList(); //Temp

            if (this.Clans == null)
            {
                this.Clans = new List<Clan>();
            }
        }

        internal override void Encode()
        {
            var _Packet = new List<byte>();
            var i = 0;

            foreach (Clan Clan in this.Clans)
            {
                _Packet.AddLong(Clan.Clan_ID);
                _Packet.AddString(Clan.Name);
                _Packet.AddInt((i++) + 1);
                _Packet.AddInt(Clan.Trophies);
                _Packet.AddInt(Resources.Random.Next(0, 10));
                _Packet.AddInt(Clan.Badge);
                _Packet.AddInt(Clan.Members.Count);
                _Packet.AddInt(0);
                _Packet.AddInt(Clan.Level);
                i++;
            }

            this.Data.AddInt(i);
            this.Data.AddRange(_Packet);

            this.Data.AddInt((int)(DateTime.UtcNow.LastDayOfMonth() - DateTime.UtcNow).TotalSeconds);

            this.Data.AddInt(3);
            this.Data.AddInt(50000);
            this.Data.AddInt(30000);
            this.Data.AddInt(15000);
        }
    }
}
