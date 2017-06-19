using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Boom.Core.Network;
using CRepublic.Boom.Extensions.Binary;
using CRepublic.Boom.Logic;
using CRepublic.Boom.Packets.Messages.Server.Alliance;

namespace CRepublic.Boom.Packets.Messages.Client.Alliance
{
    internal class Request_Create_Alliance : Message
    {
        public Request_Create_Alliance(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override void Process()
        {
            ShowValues();
            new Response_Create_Alliance(this.Device).Send();
        }
    }
}
