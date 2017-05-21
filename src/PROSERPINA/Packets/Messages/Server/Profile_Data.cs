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
            this.Data.AddHexa("06 00 3F 01 00 00 01 00 00 00 02 00 00 01 00 00 00 0E 00 00 01 00 00 00 82 01 00 00 01 00 00 00 81 01 00 00 01 00 00 00 04 00 00 01 00 00 00 00 00 00 0E 00 AC 49 2B 00 00 0E AB A4 E2 0A 0E AB A4 E2 0A 0E AB A4 E2 0A 00 00 00 00 7F 01 00 00 00 00 00 00 00 00 1F 00 00 00 00 00 07 08 05 01 A9 01 05 05 A9 01 05 1D 88 88 D5 44 05 0D 00 05 0E 00 05 03 01 05 10 01 05 02 01 00 03 3C 07 06 3C 08 06 3C 09 06 00 02 05 0B 1F 05 08 06 05 1A 00 02 1A 01 02 1C 01 02 1C 00 01 1A 0D 02 00 A4 01 A4 01 00 01 00 00 00 00 00 00 00 00 01 00 00 04 0E AB A4 E2 0A 0E AB A4 E2 0A 0E AB A4 E2 0A 00 00 00 00 7F 01 00 00 00 00 00 00 00 00 1F 00 00 00 00 00 07 08 05 01 A9 01 05 05 A9 01 05 1D 88 88 D5 44 05 0D 00 05 0E 00 05 03 01 05 10 01 05 02 01 00 03 3C 07 06 3C 08 06 3C 09 06 00 02 05 0B 1F 05 08 06 05 1A 00 02 1A 01 02 1C 01 02 1C 00 01 1A 0D 02 00 A4 01 A4 01 00 01 00 00 00 00 00 00 00 00 01 00 00 00".Replace(" ", ""));

            //this.Data.Add(3);
            //this.Data.Add(255);

            //this.Data.AddRange(this.Device.Player.Avatar.Decks.Hand());

            //this.Data.AddLong(this.Device.Player.Avatar.UserId);
            //this.Data.AddRange(this.Device.Player.Avatar.Profile);

            //this.Data.Add(0);
            //this.Data.Add(0);
            //this.Data.Add(0);
            //this.Data.Add(0);
            //this.Data.Add(0);

            //this.Data.Add(1);
        }
    }
}
