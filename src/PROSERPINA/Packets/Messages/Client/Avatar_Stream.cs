using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Messages.Client
{
    internal class Avatar_Stream : Message
    {
        public Avatar_Stream(Device _Client, Reader Reader) : base(_Client, Reader)
        {
            // Avatar_Stream.
        }
    }
}
