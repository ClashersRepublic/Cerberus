using System;
using System.Collections.Generic;
using System.Linq;
using BL.Servers.CoC.Core;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic;

namespace BL.Servers.CoC.Packets.Messages.Server.Leaderboard
{
    internal class Global_Players : Message
    {

        internal List<Level> Players;
    
        public Global_Players(Device client) : base(client)
        {
            this.Identifier = 24403;
            this.Players = Resources.PRegion.Get_Region("INTERNATIONAL").OrderByDescending(t => t.Avatar.Trophies).ToList();

            if (this.Players == null)
            {
                this.Players = new List<Level>();
            }
        }

        internal override async void Encode()
        {
            List<byte> _Packet = new List<byte>();
            int i = 0;
            foreach (var _Player in Players)
            {
                _Packet.AddLong(_Player.Avatar.UserId);
                _Packet.AddString(_Player.Avatar.Name);
                _Packet.AddInt((i++) + 1);
                _Packet.AddInt(_Player.Avatar.Trophies);
                _Packet.AddInt(Resources.Random.Next(0, 10)); //Previous month rank
                _Packet.AddInt(_Player.Avatar.Level);

                _Packet.AddInt(_Player.Avatar.Wons);
                _Packet.AddInt(_Player.Avatar.Loses);

                _Packet.AddInt(0);
                _Packet.AddInt(0);

                _Packet.AddInt(_Player.Avatar.League);
                _Packet.AddString(_Player.Avatar.Region);
                _Packet.AddLong(_Player.Avatar.UserId);
                _Packet.AddInt(3);
                _Packet.AddInt(2);
                _Packet.AddBool(_Player.Avatar.ClanId > 0);

                if (_Player.Avatar.ClanId > 0)
                {
                    var _Clan = await Resources.Clans.Get(_Player.Avatar.ClanId, Constants.Database, true);
                    _Packet.AddLong(_Player.Avatar.ClanId);
                    _Packet.AddString(_Clan.Name);
                    _Packet.AddInt(_Clan.Badge);
                }
            }

            this.Data.AddInt(i);
            this.Data.AddRange(_Packet);
            this.Data.AddInt(i);
            this.Data.AddRange(_Packet);

            this.Data.AddInt((int)(DateTime.UtcNow.LastDayOfMonth() - DateTime.UtcNow).TotalSeconds);
            this.Data.AddInt(DateTime.UtcNow.Year);
            this.Data.AddInt(DateTime.UtcNow.Month);
            this.Data.AddInt(DateTime.UtcNow.Year);
            this.Data.AddInt(DateTime.UtcNow.Month - 1);
        }
    }
}
