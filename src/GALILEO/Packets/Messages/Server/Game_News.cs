using BL.Servers.CoC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Extensions.List;

namespace BL.Servers.CoC.Packets.Messages.Server
{
    internal class Game_News : Message
    {
        public Game_News(Device Device) : base(Device)
        {
            this.Identifier = 24445;
        }

        internal override void Encode()
        {
            this.Data.AddInt(2);
            this.Data.AddInt(89);
            this.Data.AddString("icon_league_immortal"); //Icon stuff
            this.Data.AddString("Welcome to Barbarianland");
            this.Data.AddString("You don't know about BL?Pres the learn more then");
            this.Data.AddString("Learn More!");
            this.Data.AddString("https://www.google.com/");
            this.Data.AddString(null);
            this.Data.AddString(null);
            this.Data.AddString("news_entry_dynamic_color2");
            this.Data.AddByte(2);
            this.Data.AddInt(0);
            this.Data.AddString(null);
            this.Data.AddString(null);

            this.Data.AddInt(90);
            this.Data.AddString("icon_league_immortal"); //Icon stuff
            this.Data.AddString("Still don't believe me?");
            this.Data.AddString("Well suck for you");
            this.Data.AddHexa("00 00 00 0B 4C 65 61 72 6E 20 4D 6F 72 65 21 00 00 00 21 68 74 74 70 3A 2F 2F 62 69 74 2E 6C 79 2F 63 6C 61 73 68 6F 66 63 6C 61 6E 73 63 6F 6E 74 65 73 74 FF FF FF FF FF FF FF FF 00 00 00 19 6E 65 77 73 5F 65 6E 74 72 79 5F 64 79 6E 61 6D 69 63 5F 63 6F 6C 6F 72 32 00 01 34 D2 00 FF FF FF FF FF FF FF FF".Replace(" ", ""));
        }
    }
}
