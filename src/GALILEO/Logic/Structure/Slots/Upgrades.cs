using System;
using System.Collections.Generic;
using BL.Servers.CoC.Logic.Structure.Slots.Items;

namespace BL.Servers.CoC.Logic.Structure.Slots
{
    internal class Upgrades : List<Slot>, ICloneable
    {
        internal Player Player;
        
        internal Upgrades()
        {
            // Upgrades.
        }

        internal Upgrades(Player _Player)
        {
            this.Player = _Player;
        }
        internal Upgrades Clone()
        {
            return this.MemberwiseClone() as Upgrades;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}