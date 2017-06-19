using Republic.Magic.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Republic.Magic.Extensions.List;

namespace Republic.Magic.Packets.Messages.Server
{
    internal class Game_News : Message
    {
        public Game_News(Device Device) : base(Device)
        {
            this.Identifier = 24445;
        }

        internal override void Encode()
        {
            this.Data.AddInt(1);
            this.Data.AddInt(89);
            this.Data.AddString("icon_league_immortal"); //Icon stuff
            this.Data.AddString("Welcome to Barbarianland");
            this.Data.AddString("You don't know about BL? Fuck your self then");
            this.Data.AddString("Learn More!");
            this.Data.AddString("https://www.google.com/");
            this.Data.AddString(null);
            this.Data.AddString(null);
            this.Data.AddString("news_entry_dynamic_color2");
            this.Data.AddByte(0);
            this.Data.AddInt(0);
            this.Data.AddString(null);
            this.Data.AddString(null);

        }
    }
}
