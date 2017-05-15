using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CoC.Packets.Messages.Server.API
{
    internal class Friend_List_Data : Message
    {
        public Friend_List_Data(Device Device) : base(Device)
        {
            this.Identifier = 20105;
        }

        internal override void Encode()
        {
            this.Data.AddInt(0);
            this.Data.AddInt(2);

            this.Data.AddLong(1);
            this.Data.AddBool(true);
            this.Data.AddLong(1);
            this.Data.AddString("Test Facebook");
            this.Data.AddString("816866385137342");
            this.Data.AddString(null);
            this.Data.AddInt(0);
            this.Data.AddInt(99);
            this.Data.AddInt(0); //League?
            this.Data.AddInt(9999);
            this.Data.AddString(null);
            this.Data.AddInt(0);
            this.Data.AddBool(true);
            this.Data.AddLong(2);
            this.Data.AddInt(1795175765); //Badge
            this.Data.AddString("Test Clan");
            this.Data.AddInt(2);
            this.Data.AddInt(10);
            this.Data.AddBool(false);

            this.Data.AddLong(2);
            this.Data.AddBool(true);
            this.Data.AddLong(2);
            this.Data.AddString("Test Facebook 2");
            this.Data.AddString("816866385137343");
            this.Data.AddString(null);
            this.Data.AddInt(0);
            this.Data.AddInt(99);
            this.Data.AddInt(0); //League?
            this.Data.AddInt(9999);
            this.Data.AddString(null);
            this.Data.AddInt(0);
            this.Data.AddBool(false);

        }
    }
}
