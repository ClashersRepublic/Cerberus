using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CoC.Packets.Commands.Server
{
    internal class Changed_Alliance_Setting : Command
    {
        internal Clan Clan;

        public Changed_Alliance_Setting(Device client) : base(client)
        {
            this.Identifier = 6;
        }

        internal override void Encode()
        {
            this.Data.AddLong(this.Clan.Clan_ID);
            this.Data.AddInt(this.Clan.Badge);
            this.Data.AddInt(this.Clan.Level);
            this.Data.AddInt(-1); 
        }
    }
}