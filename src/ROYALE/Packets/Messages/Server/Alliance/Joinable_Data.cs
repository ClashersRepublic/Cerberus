using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Extensions.List;
using CRepublic.Royale.Core;

namespace CRepublic.Royale.Packets.Messages.Server.Alliance
{
    internal class Joinable_Data : Message
    {
        public Joinable_Data(Device Device) : base(Device)
        {
            this.Identifier = 24304;
        }

        internal override void Encode()
        {
            this.Data.AddVInt((int)Server_Resources.Clans.Count);

            foreach (var _Clan in Server_Resources.Clans.Values)
            {
                this.Data.AddRange(_Clan.FullHeader);
            }
        }
    }
}
