using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Messages.Server.Alliance
{
    internal class Joinable_Data : Message
    {
        public Joinable_Data(Device Device) : base(Device)
        {
            this.Identifier = 24304;
        }

        internal override void Encode()
        {
            List<Clan> Clans = new List<Clan>();

            foreach (Clan _Clan in Clans)
            {
                this.Data.AddRange(_Clan.FullHeader());
            }
        }
    }
}
