using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Core.Network;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;
using BL.Servers.CR.Packets.Messages.Server.Alliance;

namespace BL.Servers.CR.Packets.Messages.Client.Alliance
{
    internal class Request_Joinable : Message
    {
        public Request_Joinable(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal override void Process()
        {
            //new Joinable_Data(this.Device).Send();
        }
    }
}
