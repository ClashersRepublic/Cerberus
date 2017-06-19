using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Core;
using CRepublic.Royale.Core.Network;
using CRepublic.Royale.Extensions.Binary;
using CRepublic.Royale.Files;
using CRepublic.Royale.Files.CSV_Logic;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Logic.Enums;
using CRepublic.Royale.Packets.Messages.Server.Alliance;
using Server_Resources = CRepublic.Royale.Core.Server_Resources;

namespace CRepublic.Royale.Packets.Messages.Client.Alliance
{
    internal class Request_Create_Alliance : Message
    {
        internal string Name = string.Empty;
        internal string Description = string.Empty;

        internal int Badge = 0;
        internal int Origin = 0;
        internal int Required_Score = 0;
        internal int Type = 0;

        internal Clan Clan = Server_Resources.Clans.New(0, Constants.Database, true);

        public Request_Create_Alliance(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override void Decode()
        {
            this.Clan.Name = this.Reader.ReadString();
            this.Clan.Description = this.Reader.ReadString();
            this.Reader.ReadVInt();
            this.Clan.Badge = this.Reader.ReadVInt();
            this.Clan.Type = (Clan_Type)this.Reader.ReadVInt();
            this.Clan.Required_Score = this.Reader.ReadVInt();
            this.Reader.ReadVInt();
            this.Clan.Origin = this.Reader.ReadVInt();
        }

        internal override void Process()
        {
            int Cost = (CSV.Tables.Get(Gamefile.Globals).GetData("ALLIANCE_CREATE_COST") as Globals).NumberValue;

            if (this.Device.Player.HasEnoughResources(Game_Resource.Gold, Cost))
            {
                this.Device.Player.Resources.Minus(Game_Resource.Gold, Cost);

                this.Device.Player.ClanId = this.Clan.ClanID;

                this.Clan.Members.Add(this.Device.Player);

                new Response_Create_Alliance(this.Device, this.Clan, 142, true).Send();
            }

            Server_Resources.Clans.Save(this.Clan);
        }
    }
}
