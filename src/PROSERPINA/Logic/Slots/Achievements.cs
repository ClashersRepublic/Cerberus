using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Logic.Slots.Items;

namespace BL.Servers.CR.Logic.Slots
{
    internal class Achievements : List<Achievement>
    {
        internal Achievements()
        {
            
        }

        internal List<Achievement> Completed
        {
            get { return this.Where(Achievement => Achievement.Value == -1).ToList(); }
        }

        internal new void Add(Slot Achievement)
        {
            if (this.FindIndex(A => A.Data == Achievement.Data) < 0)
                this.Add(Achievement);
        }
    }
}
