using CRepublic.Magic.Core;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.List;
using CRepublic.Magic.Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CRepublic.Magic.Packets.Messages.Server.Leaderboard
{
    internal class Local_Players : Message
    {
        internal List<Level> Players;

        public Local_Players(Device client) : base(client)
        {
            this.Identifier = 24404;
            this.Players = Resources.PRegion.Get_Region(this.Device.Player.Avatar.Region)?.Take(100).OrderByDescending(t => t.Avatar.Trophies).ToList();

            if (this.Players == null)
            {
                this.Players = new List<Level>();
            }

        }

        internal override void Encode()
        {
            this.Data.AddInt(this.Players.Count);

            int i = 1;
            foreach (var _Player in Players)
            {
                this.Data.AddLong(_Player.Avatar.UserId);
                this.Data.AddString(_Player.Avatar.Name);
                this.Data.AddInt(i++);
                this.Data.AddInt(_Player.Avatar.Trophies);
                this.Data.AddInt(Resources.Random.Next(0, 10)); //Previous month rank
                this.Data.AddInt(_Player.Avatar.Level);

                this.Data.AddInt(_Player.Avatar.Wons);
                this.Data.AddInt(_Player.Avatar.Loses);

                this.Data.AddInt(0);
                this.Data.AddInt(0);

                this.Data.AddInt(_Player.Avatar.League);
                this.Data.AddString(_Player.Avatar.Region);
                this.Data.AddLong(_Player.Avatar.UserId);
                this.Data.AddInt(3);
                this.Data.AddInt(2);
                this.Data.AddBool(_Player.Avatar.ClanId > 0);

                if (_Player.Avatar.ClanId > 0)
                {
                    var _Clan = Resources.Clans.Get(_Player.Avatar.ClanId);
                    this.Data.AddLong(_Player.Avatar.ClanId);
                    this.Data.AddString(_Clan.Name);
                    this.Data.AddInt(_Clan.Badge);
                }
            }
        }
    }
}
