using System.Collections.Generic;
using CRepublic.Royale.Extensions.List;
using CRepublic.Royale.Logic.Structure.Slots.Items;

namespace CRepublic.Royale.Logic.Structure.Slots
{
    internal class Variables : List<Slot>
    {
        internal Player Player;

        internal Variables()
        {
            // Variables.
        }

        internal Variables(Player _Player, bool Initialize = false)
        {
            this.Player = _Player;

            if (Initialize)
                this.Initialize();
        }

        internal bool IsBuilderVillage => this.Get(Enums.Variable.VillageToGoTo) == 1;

        internal int Get(int Gl_ID)
        {
            int i = this.FindIndex(R => R.Data == Gl_ID);

            if (i > -1)
                return this[i].Count;

            return 0;
        }

        internal int Get(Enums.Variable Variables)
        {
            return this.Get(37000000 + (int)Variables);
        }

        internal void Set(int Global, int Count)
        {
            int i = this.FindIndex(R => R.Data == Global);

            if (i > -1)
                this[i].Count = Count;
            else
                this.Add(new Slot(Global, Count));
        }

        internal void Set(Enums.Variable Variables, int Count)
        {
            this.Set(37000000 + (int)Variables, Count);
        }

        internal void Plus(int Global, int Count)
        {
            int i = this.FindIndex(R => R.Data == Global);

            if (i > -1)
                this[i].Count += Count;
            else this.Add(new Slot(Global, Count));
        }

        internal void Plus(Enums.Variable Resource, int Count)
        {
            this.Plus(37000000 + (int)Resource, Count);
        }

        internal bool Minus(int Global, int Count)
        {
            int i = this.FindIndex(R => R.Data == Global);

            if (i > -1)
                if (this[i].Count >= Count)
                {
                    this[i].Count -= Count;
                    return true;
                }

            return false;
        }

        internal void Minus(Enums.Variable _Resource, int _Value)
        {
            int Index = this.FindIndex(T => T.Data == 37000000 + (int)_Resource);

            if (Index > -1)
            {
                this[Index].Count -= _Value;
            }
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> Packet = new List<byte>();

                foreach (Slot Resource in this)
                {
                    Packet.AddInt(Resource.Data);
                    Packet.AddInt(Resource.Count);
                }

                return Packet.ToArray();
            }
        }

        internal void Initialize()
        {
            this.Set(Logic.Enums.Variable.AccountBound, 0);
            this.Set(Logic.Enums.Variable.BeenInArrangedWar, 0);
            this.Set(Logic.Enums.Variable.ChallengeLayoutIsWar, 0);
            this.Set(Logic.Enums.Variable.ChallengeStarted, 0);
            this.Set(Logic.Enums.Variable.EventUseTroop, 0);
            this.Set(Logic.Enums.Variable.FILL_ME, 0);
            this.Set(Logic.Enums.Variable.FriendListLastOpened, 0);
            this.Set(Logic.Enums.Variable.LootLimitCooldown, 0);
            this.Set(Logic.Enums.Variable.LootLimitFreeSpeedUp, 0);
            this.Set(Logic.Enums.Variable.LootLimitTimerEndSubTick, 0);
            this.Set(Logic.Enums.Variable.LootLimitTimerEndTimestamp, 0);
            this.Set(Logic.Enums.Variable.LootLimitWinCount, 0);
            this.Set(Logic.Enums.Variable.SeenBuilderMenu, 0);
            this.Set(Logic.Enums.Variable.StarBonusCooldown, 0);
            this.Set(Logic.Enums.Variable.StarBonusCounter, 0);
            this.Set(Logic.Enums.Variable.StarBonusTimerEndSubTick, 0);
            this.Set(Logic.Enums.Variable.StarBonusTimerEndTimestep, 0);
            this.Set(Logic.Enums.Variable.StarBonusTimesCollected, 0);
            this.Set(Logic.Enums.Variable.VillageToGoTo, 0);
            this.Set(Logic.Enums.Variable.Village2BarrackLevel, 0);
        }
    }
}