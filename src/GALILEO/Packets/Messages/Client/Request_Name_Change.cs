using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Packets.Messages.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CoC.Packets.Messages.Client
{
    internal class Request_Name_Change : Message
    {
        internal string Name;
        public Request_Name_Change(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Request_Name_Change.
        }
        internal override void Decode()
        {
            this.Name = this.Reader.ReadString();
        }
        internal override void Process()
        {
            new Name_Change_Response(this.Device, this.Name).Send();
        }
    }
}
