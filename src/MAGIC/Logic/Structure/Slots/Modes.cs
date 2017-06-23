using System.Collections.Generic;
using CRepublic.Magic.Logic.Structure.Slots.Items;

namespace CRepublic.Magic.Logic.Structure.Slots
{
    internal class Modes : List<Slot>
    {
        internal Player Player;

        internal Modes()
        {
            // Modes.
        }

        internal Modes(Player _Player, bool Initialize = false)
        {
            this.Player = _Player;

            if (Initialize)
                this.Initialize();
        }

        internal bool IsAttackingOwnBase => this.Get(Enums.Mode.ATTACK_OWN_BASE) == 1;

        internal int Get(int Gl_ID)
        {
            int i = this.FindIndex(R => R.Data == Gl_ID);

            if (i > -1)
                return this[i].Count;

            return 0;
        }

        internal int Get(Enums.Mode Variables)
        {
            return this.Get((int)Variables);
        }

        internal void Set(int Global, int Count)
        {
            int i = this.FindIndex(R => R.Data == Global);

            if (i > -1)
                this[i].Count = Count;
            else
                this.Add(new Slot(Global, Count));
        }

        internal void Set(Enums.Mode Variables, int Count)
        {
            this.Set((int)Variables, Count);
        }

        internal void Minus(Enums.Mode _Resource, int _Value)
        {
            int Index = this.FindIndex(T => T.Data == (int)_Resource);

            if (Index > -1)
            {
                this[Index].Count -= _Value;
            }
        }

        internal void Initialize()
        {
            this.Set(Enums.Mode.ATTACK_OWN_BASE, 0);
        }
    }
}