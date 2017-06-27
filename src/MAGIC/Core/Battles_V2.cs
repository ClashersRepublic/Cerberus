using System;
using System.Collections.Generic;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Structure.Slots;
using System.Collections.Concurrent;

namespace CRepublic.Magic.Core
{
    internal class Battles_V2 : ConcurrentDictionary<long, Battle_V2>
    {
        internal int Seed = 1;

        internal List<Level> Waiting = null;

        public Battles_V2()
        {
            this.Waiting = new List<Level>();
        }

        public void Remove(long BattleID)
        {
            if (this.ContainsKey(BattleID))
            {
            }
        }

        public void Enqueue(Level Player)
        {
            lock (this.Waiting)
            {
                this.Waiting.Add(Player);
            }
        }

        public void Dequeue(Level Player)
        {
            lock (this.Waiting)
            {
                this.Waiting.Remove(Player);
            }
        }

        public Level Dequeue()
        {
            Level _Player = null;

            lock (this.Waiting)
            {
                _Player = this.Waiting[0];
                this.Waiting.RemoveAt(0);
            }

            return _Player;
        }

        public Logic.Structure.Slots.Items.Battle_V2 GetEnemy(long BattleID, long UserID)
        {
            if (this.ContainsKey(BattleID))
            {
                return this[BattleID].Player1.Avatar.UserId != UserID ? this[BattleID].Battle1 : this[BattleID].Battle2;
            }
            return null;
        }

        public Logic.Structure.Slots.Items.Battle_V2 GetPlayer(long BattleID, long UserID)
        {
            if (this.ContainsKey(BattleID))
            {
                return this[BattleID].Player1.Avatar.UserId == UserID ? this[BattleID].Battle1 : this[BattleID].Battle2;
            }
            return null;
        }
    }
}
