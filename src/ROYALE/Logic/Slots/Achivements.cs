using System;
using System.Collections.Generic;
using System.Linq;
using CRepublic.Royale.Logic.Slots.Items;

namespace CRepublic.Royale.Logic.Slots
{
    internal class Achievements : List<Achievement>
    {
        internal Achievements()
        {
            // Achievements.
        }

        internal List<Achievement> Completed
        {
            get
            {
                return this.Where(Achievement => Achievement.Value > 0).ToList();
            }
        }

        internal new void Add(Achievement Achievement)
        {
            if (this.FindIndex(A => A.Value == Achievement.Value) < 0)
                this.Add(Achievement);
            else
                this[Achievement.Identifier].Value++;
        }
    }
}