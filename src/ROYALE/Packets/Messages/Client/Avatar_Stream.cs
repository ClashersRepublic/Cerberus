using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Extensions.Binary;
using CRepublic.Royale.Logic;

namespace CRepublic.Royale.Packets.Messages.Client
{
    internal class Avatar_Stream : Message
    {
        public Avatar_Stream(Device _Client, Reader Reader) : base(_Client, Reader)
        {
            // Avatar_Stream.
        }
    }
}
