using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Republic.Magic.Core.Networking;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Logic;
using Republic.Magic.Packets.Messages.Server.Clans.War;

namespace Republic.Magic.Packets.Messages.Client.Clans.War
{
    internal class Request_War_Home_Data : Message
    {
        public Request_War_Home_Data(Device Device, Reader Reader) : base(Device, Reader)
        {

        }

        internal override void Process()
        {
            new Visit_War_Player(this.Device).Send();
        }
    }
}
