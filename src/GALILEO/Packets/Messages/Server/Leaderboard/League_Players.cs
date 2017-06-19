using System;
using System.Collections.Generic;
using System.Linq;
using CRepublic.Magic.Core;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.List;
using CRepublic.Magic.Logic;

namespace CRepublic.Magic.Packets.Messages.Server.Leaderboard
{

    internal class League_Players : Message
    {
        internal List<Level> Players;
        public League_Players(Device client) : base(client)
        {
            this.Identifier = 24503;
            this.Players = Resources.Players.Values.Where(t => t.Avatar.League == this.Device.Player.Avatar.League).OrderByDescending(t => t.Avatar.Trophies).Take(100).ToList();

            if (this.Players == null)
            {
                this.Players = new List<Level>();
            }
        }

        internal override void Encode()
        {
            var i = 1;

            this.Data.AddInt((int)(DateTime.UtcNow.LastDayOfMonth() - DateTime.UtcNow).TotalSeconds);
            this.Data.AddInt(this.Players.Count);

            foreach (var Player in this.Players)
            {
                this.Data.AddLong(Player.Avatar.UserId);
                this.Data.AddString(Player.Avatar.Name);
                this.Data.AddInt(i++);
                this.Data.AddInt(Player.Avatar.Trophies);
                this.Data.AddInt(Resources.Random.Next(0, 10)); //Previous month rank
                this.Data.AddInt(Player.Avatar.Level);
                this.Data.AddInt(Player.Avatar.Wons);
                this.Data.AddInt(Player.Avatar.Loses);
                this.Data.AddInt(100);
                this.Data.AddInt(1);
                this.Data.AddLong(Player.Avatar.UserId);
                this.Data.AddInt(3);
                this.Data.AddInt(2);
                this.Data.AddBool(Player.Avatar.ClanId > 0);
                if (Player.Avatar.ClanId > 0)
                {
                    var _Clan = Resources.Clans.Get(Player.Avatar.ClanId, Constants.Database, true);
                    this.Data.AddLong(Player.Avatar.ClanId);
                    this.Data.AddString(_Clan.Name);
                    this.Data.AddInt(_Clan.Badge);
                }
                i++;

            }
        }
    }
}
