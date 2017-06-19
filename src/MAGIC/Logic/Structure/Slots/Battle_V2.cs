using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRepublic.Magic.Logic.Structure.Slots
{
    internal class Battle_V2
    {
        internal int BattleID;
        internal int Tick;
        internal int Checksum;
        internal bool Started;

        internal Level Player1;
        internal Level Player2;

        internal Items.Battle_V2 Battle1;
        internal Items.Battle_V2 Battle2;
        
        internal Timer Timer = new Timer();

        public Battle_V2()
        {

        }

        public Battle_V2(Level _Player1, Level _Player2)
        {
            this.Player1 = _Player1;
            this.Player2 = _Player2;
            this.Battle1 = new Items.Battle_V2(_Player1, _Player2);
            this.Battle2 = new Items.Battle_V2(_Player2, _Player1);

        }
    }
}
