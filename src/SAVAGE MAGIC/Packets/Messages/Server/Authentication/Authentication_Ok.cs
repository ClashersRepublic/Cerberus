using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.List;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;

namespace CRepublic.Magic.Packets.Messages.Server.Authentication
{
    internal class Authentication_OK : Message
    {
        public Authentication_OK(Device client) : base(client)
        {
            this.Identifier = 20104;
            this.Device.State = State.LOGGED;
        }

        internal override void Encode()
        {
            this.Data.AddLong(1);
            this.Data.AddLong(1);

            this.Data.AddString("");

            this.Data.AddString("");
            this.Data.AddString("");


            this.Data.AddInt(9);
            this.Data.AddInt(105);
            this.Data.AddInt(7);

            this.Data.AddString("prod");

            this.Data.AddInt(0); //Session Count
            this.Data.AddInt(0); //Playtime Second
            this.Data.AddInt(0);

            this.Data.AddString("");

            this.Data.AddString("");
            this.Data.AddString("");

            this.Data.AddInt(0);
            this.Data.AddString("");
            this.Data.AddString("");
            this.Data.AddString(null);
            this.Data.AddInt(1); //Unknown
            this.Data.AddString("https://www.clashersrepublic.com/events/");
            this.Data.AddString("http://b46f744d64acd2191eda-3720c0374d47e9a0dd52be4d281c260f.r11.cf2.rackcdn.com/"); //Patch server?
            this.Data.AddString(null);
        }
    }
}
