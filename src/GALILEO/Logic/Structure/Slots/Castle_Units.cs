using System;
using Republic.Magic.Logic.Structure.Slots.Items;
using System.Collections.Generic;

namespace Republic.Magic.Logic.Structure.Slots
{
    internal class Castle_Units : List<Alliance_Unit>, ICloneable
    {
        internal Player Player;

        internal Castle_Units()
        {
        }

        internal Castle_Units(Player _Player)
        {
            this.Player = _Player;
        }

        internal Castle_Units Clone()
        {
            return this.MemberwiseClone() as Castle_Units;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}