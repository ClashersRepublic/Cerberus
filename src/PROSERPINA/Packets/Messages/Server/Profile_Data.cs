using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Extensions.List;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Messages.Server
{
    internal class Profile_Data : Message
    {
        public Profile_Data(Device Device) : base(Device)
        {
            this.Identifier = 24113;
        }

        internal override void Encode()
        {
            this.Data.Add(5);
            this.Data.AddHexa("00FF");
            this.Data.AddRange(this.Device.Player.Decks.Hand());

            this.Data.AddLong(this.Device.Player.UserId);
            this.Data.Add(0);
            this.Data.Add(0);
            this.Data.AddRange(this.Device.Player.Profile.ToBytes);
            this.Data.Add(4);
            this.Data.AddRange(this.Device.Player.Profile.ToBytes);
            this.Data.Add(0);
        }
    }
}
