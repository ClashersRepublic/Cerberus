using System.Collections.Generic;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Logic.Slots.Items;

namespace CRepublic.Royale.Core.Server_Components
{ 
    internal class Battles : Dictionary<long, Battle>
    {
        internal int Seed = 1;

        internal List<Player> Waiting = null;

        public Battles()
        {
            this.Waiting = new List<Player>();
        }

        public new void Add(Battle Battle)
        {
            if (this.ContainsKey(Battle.BattleID))
            {
                this[Battle.BattleID] = Battle;
            }
            else
            {
                this.Add(Battle.BattleID, Battle);
            }
        }

        public new void Remove(long BattleID)
        {
            if (this.ContainsKey(BattleID))
            {
                // Set player states to logged.
            }
        }

        public void Enqueue(Player Player)
        {
            lock (this.Waiting)
            {
                this.Waiting.Add(Player);
            }
        }

        public void Dequeue(Player Player)
        {
            lock (this.Waiting)
            {
                this.Waiting.Remove(Player);
            }
        }

        public Player Dequeue()
        {
            Player _Player = null;

            lock (this.Waiting)
            {
                _Player = this.Waiting[0];
                this.Waiting.RemoveAt(0);
            }

            return _Player;
        }

        public Player GetEnemy(long BattleID, long UserID)
        {
            if (this.ContainsKey(BattleID))
            {
                return this[BattleID].Player1.UserId == UserID ? this[BattleID].Player2 : this[BattleID].Player1;
            }    
            return null;
        }
    }
}
