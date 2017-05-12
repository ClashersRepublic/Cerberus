using System;
using System.Collections.Generic;
using BL.Servers.CoC.Logic.Structure.Slots.Items;

namespace BL.Servers.CoC.Logic.Structure.Slots
{
    internal class Slots : List<Slot>, ICloneable
    {
        internal Slots Clone()
        {
            return this.MemberwiseClone() as Slots;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}