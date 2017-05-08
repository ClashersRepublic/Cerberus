using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Structure.Slots.Items;
using System.Collections.Generic;

namespace BL.Servers.CoC.Core
{
    internal class Player_Region : Dictionary<string, List_Regions>
    {
        internal object Gate = new object();
        internal object GateAdd = new object();

        internal void Add(Level Player)
        {
            lock (this.GateAdd)
            {
                if (!string.IsNullOrEmpty(Player.Avatar.Region))
                {
                    if (this.ContainsKey(Player.Avatar.Region))
                    {
                        this[Player.Avatar.Region].Level.Add(Player);
                    }
                    else
                    {
                        this.Add(Player.Avatar.Region, new List_Regions(Player));
                    }
                }
            }
        }

        internal void Remove(Level Player)
        {
            if (!string.IsNullOrEmpty(Player.Avatar.Region))
            {
                if (this.ContainsKey(Player.Avatar.Region))
                {
                    this[Player.Avatar.Region].Remove(Player);
                    if (this[Player.Avatar.Region].Level.Count < 1)
                        this.Remove(Player.Avatar.Region);
                }
            }
        }

        internal List<Level> Get_Region(Level Player)
        {
            if (!string.IsNullOrEmpty(Player.Avatar.Region))
            {
                if (this.ContainsKey(Player.Avatar.Region))
                {
                    return this[Player.Avatar.Region].Level;
                }
                this.Add(Player.Avatar.Region, new List_Regions(Player));
                return this[Player.Avatar.Region].Level;
            }
            return null;
        }
    }
}
