using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Core.Network;
using CRepublic.Royale.Extensions.Binary;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Packets.Messages.Server.Alliance;

namespace CRepublic.Royale.Packets.Messages.Client.Alliance
{
    internal class Request_Joinable : Message
    {
        public Request_Joinable(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal override void Decode()
        {
        }

        internal override void Process()
        {
            new Joinable_Data(this.Device).Send();
        }
    }
}
