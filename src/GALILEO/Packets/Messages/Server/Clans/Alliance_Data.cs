using System;
using BL.Servers.CoC.Core;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Packets.Messages.Server.Errors;

namespace BL.Servers.CoC.Packets.Messages.Server.Clans
{
    internal class Alliance_Data : Message
    {
        internal Clan Clan = null;
        internal long ClanID = 0;

        public Alliance_Data(Device Device) : base(Device)
        {
            this.Identifier = 24301;
        }

        public Alliance_Data(Device Device, Clan clan) : base(Device)
        {
            this.Identifier = 24301;
            this.Clan = clan;
        }
        
        internal override void Encode()
        {
            if (Clan == null)
                Clan = Resources.Clans.Get(this.ClanID == 0 ? this.Device.Player.Avatar.ClanId : this.ClanID , Constants.Database, false);


            if (Clan != null)
            {
                this.Data.AddRange(Clan.ToBytes);

                this.Data.AddString(Clan.Description);
                this.Data.AddInt(6);
                this.Data.AddBool(false);
                this.Data.AddInt(0);
                this.Data.AddByte(0);
                this.Data.AddRange(Clan.Members.ToBytes);
                this.Data.AddInt(0);
                this.Data.AddInt(32);
            }
            else
            {
                new Out_Of_Sync(this.Device).Send();
            }
        }
    }
}