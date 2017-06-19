using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Packets.Messages.Server.Clans.War;

namespace CRepublic.Magic.Packets.Messages.Client.Clans.War
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
