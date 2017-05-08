using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Logic.Slots.Items
{
    internal class Card
    {
        internal int Count = 0;

        internal byte ID = 0;
        internal byte Level = 0;
        internal byte New = 0;
        internal byte Type = 0;

        public Card()
        {
            
        }

        public Card(byte _Type, byte _ID, int _Count, byte _Level, byte _isNew)
        {
            this.Type = _Type;
            this.ID = _ID;
            this.Count = _Count;
            this.Level = _Level;
            this.New = _isNew;
        }

        public int GlobalID => this.Type * 1000000 + this.ID;
    }
}
