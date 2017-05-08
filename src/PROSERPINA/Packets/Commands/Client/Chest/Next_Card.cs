using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Commands.Client.Chest
{
    internal class Next_Card : Command
    {
        internal int Tick = 0;

        public Next_Card(Reader _Reader, Device _Client, int _ID) : base(_Reader, _Client, _ID)
        {
            // Chest_Next_Card.
        }

        internal override void Decode()
        {
            this.Tick = this.Reader.ReadRRInt32();
            this.Tick = this.Reader.ReadRRInt32();

            this.Reader.ReadInt16();
        }
    }
}
