using System;
using System.Collections.Generic;
using BL.Servers.CoC.Logic.Structure.Slots.Items;

namespace BL.Servers.CoC.Logic.Structure.Slots
{
    internal class Battle_Commands : List<Battle_Command>
    {
        public void Add(Battle Battle, Battle_Command Command)
        {
            if (Battle.Preparation_Time > 0)
                Battle.Preparation_Skip = (int)Math.Round(Battle.Preparation_Time);

            this.Add(Command);
        }
    }
}