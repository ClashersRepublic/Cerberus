using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Extensions.List;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Messages.Server
{
    internal class Inbox_Data : Message
    {
        internal Inbox_Data(Device Device) : base(Device)
        {
            this.Identifier = 24445;
        }

        internal override void Encode()
        {
            this.Data.AddInt(1);

            this.Data.AddString("https://56f230c6d142ad8a925f-b174a1d8fb2cf6907e1c742c46071d76.ssl.cf2.rackcdn.com/inbox/ClashRoyale_logo_small.png");
            this.Data.AddString("Hi Papertown");
            this.Data.AddString("Welcome to Papertown !");
            this.Data.AddString("About us");
            this.Data.AddString("https://www.google.com/");
            this.Data.AddString("https://cdn.discordapp.com/icons/229282495852445698/05d6f76415a7a59a6c312f2936f56def.jpg?size=128");
            this.Data.AddString("https://cdn.discordapp.com/icons/229282495852445698/05d6f76415a7a59a6c312f2936f56def.jpg?size=128");
            this.Data.AddString("http://<asset_path_update>");
        }
    }
}
