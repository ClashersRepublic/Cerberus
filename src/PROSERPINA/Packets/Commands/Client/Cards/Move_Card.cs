using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Commands.Client.Cards
{
    internal class Move_Card : Command
    {
        internal int Tick = 0;
        internal int Position = 0;
        internal int ID;

        public Move_Card(Reader Reader, Device Device, int _ID) : base(Reader, Device, _ID)
        {
            
        }

        internal override void Decode()
        {
            this.Tick = this.Reader.ReadVInt();
            this.Tick = this.Reader.ReadVInt();

            this.Reader.ReadInt16();

            this.ID = this.Reader.ReadVInt();
            this.Position = this.Reader.ReadVInt();
        }

        internal override void Process()
        {
            this.Device.Player.Decks.Invert(this.ID, this.Position);
        }
    }
}
