using BL.Servers.CoC.Core;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic.Enums;

namespace BL.Servers.CoC.Packets.Messages.Server.Clans
{
    internal class Alliance_Full_Entry : Message
    {
        internal Clan Clan = null;
        internal int ClanID = 0;

        public Alliance_Full_Entry(Device Device) : base(Device)
        {
            this.Identifier = 24324;
        }

        public Alliance_Full_Entry(Device Device, Clan clan) : base(Device)
        {
            this.Identifier = 24324;
            this.Clan = clan;
        }

        internal override async void Encode()
        {
            if (Clan == null)
                Clan = await Resources.Clans.Get(this.ClanID == 0 ? this.Device.Player.Avatar.ClanId : this.ClanID, Constants.Database, false);

            this.Data.AddString(this.Clan.Description);
            this.Data.AddInt((int)WarState.NONE); //War state:

            this.Data.AddInt(0); 

            this.Data.Add(0); 
            //pack.AddLong(WarID);

            this.Data.Add(0);
            this.Data.AddInt(0);
            this.Data.AddRange(Clan.ToBytes);
        }
    }
}
