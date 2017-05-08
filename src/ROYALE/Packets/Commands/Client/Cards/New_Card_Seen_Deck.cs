using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Commands.Client.Cards
{
    internal class New_Card_Seen_Deck : Command
    {
        internal int Tick = 0;

        public New_Card_Seen_Deck(Reader Reader, Device Device, int ID) : base(Reader, Device, ID)
        {
        }

        internal override void Decode()
        {
            this.Tick = this.Reader.ReadRRInt32();
            this.Tick = this.Reader.ReadRRInt32();
            this.Reader.ReadInt16();
        }
    }
}
