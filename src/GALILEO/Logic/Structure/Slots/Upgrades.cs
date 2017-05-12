using System.Collections.Generic;
using BL.Servers.CoC.Logic.Structure.Slots.Items;

namespace BL.Servers.CoC.Logic.Structure.Slots
{
    internal class Upgrades : List<Slot>
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
    }
}