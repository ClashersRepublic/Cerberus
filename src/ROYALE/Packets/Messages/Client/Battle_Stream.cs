using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Extensions.Binary;
using CRepublic.Royale.Logic;

namespace CRepublic.Royale.Packets.Messages.Client
{
    internal class Battle_Stream : Message
    {
        public Battle_Stream(Device _Client, Reader Reader) : base(_Client, Reader)
        {
            // Battle_Stream.
        }

        internal override void Decode()
        {
        }

        internal override void Process()
        {
        }
    }
}
