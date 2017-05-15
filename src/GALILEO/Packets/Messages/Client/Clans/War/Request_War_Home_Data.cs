using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Packets.Messages.Server.Clans.War;

namespace BL.Servers.CoC.Packets.Messages.Client.Clans.War
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
