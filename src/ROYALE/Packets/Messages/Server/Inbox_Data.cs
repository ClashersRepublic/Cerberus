using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Extensions.List;
using CRepublic.Royale.Logic;

namespace CRepublic.Royale.Packets.Messages.Server
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
            this.Data.AddString("Welcome to <c4>BarbarianLand</c>!");
            this.Data.AddString("Click the 'Visit Site' button to check out our site!");
            this.Data.AddString("Visit site!");
            this.Data.AddString("https://www.barbarianland.com");
            this.Data.AddString("");
            this.Data.AddString("");
            this.Data.AddString("");
        }
    }
}
