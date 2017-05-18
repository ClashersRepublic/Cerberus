using BL.Servers.CoC.Core;
using BL.Servers.CoC.Core.Database;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.Servers.CoC.Packets.Messages.Server.API
{
    internal class Friend_List_Data : Message
    {
        internal List<Level> Players;
        public Friend_List_Data(Device Device, List<string> ID) : base(Device)
        {
            this.Identifier = 20105;
            this.Players = MySQL_V2.GetPlayerViaFID(ID);
            this.Players = this.Players?.OrderByDescending(t => t.Avatar.Trophies).ToList();
        }

        internal override void Encode()
        {
            this.Data.AddInt(0);
            this.Data.AddInt(this.Players.Count);
            
            foreach(var Player in Players)
            {
                this.Data.AddLong(Player.Avatar.UserId);
                this.Data.AddBool(true);
                this.Data.AddLong(Player.Avatar.UserId);
                this.Data.AddString(Player.Avatar.Name);
                this.Data.AddString(Player.Avatar.Facebook.Identifier);
                this.Data.AddString(Player.Avatar.Google.Identifier);
                this.Data.AddInt(26180);
                this.Data.AddInt(Player.Avatar.Level);
                this.Data.AddInt(Player.Avatar.League);
                this.Data.AddInt(Player.Avatar.Trophies);
                this.Data.AddString(null);
                this.Data.AddInt(0);

                this.Data.AddBool(Player.Avatar.ClanId > 0);

                if (Player.Avatar.ClanId > 0)
                {
                    Logic.Clan _Clan = Resources.Clans.Get(Player.Avatar.ClanId, Constants.Database, false);
                    this.Data.AddLong(Player.Avatar.ClanId);
                    this.Data.AddInt(_Clan.Badge);
                    this.Data.AddString(_Clan.Name);
                    this.Data.AddInt((int)_Clan.Members[Player.Avatar.UserId].Role);
                    this.Data.AddInt(_Clan.Level);
                    this.Data.AddBool(false);
                }

            }
        }
    }
}
