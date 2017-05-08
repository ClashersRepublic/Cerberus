using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Files;
using BL.Servers.CoC.Files.CSV_Logic;
using BL.Servers.CoC.Logic.Structure.Slots.Items;

namespace BL.Servers.CoC.Logic.Structure.Slots
{
    internal class Resources : List<Slot>
    {
        internal Player Player;

        internal Resources()
        {
            // Resources.
        }

        internal Resources(Player _Player, bool Initialize = false)
        {
            this.Player = _Player;

            if (Initialize)
                this.Initialize();
        }

        internal int Gems => this.Get(Enums.Resource.Diamonds);

        internal int Get(int Gl_ID)
        {
            int i = this.FindIndex(R => R.Data == Gl_ID);

            if (i > -1)
                return this[i].Count;

            return 0;
        }

        internal int Get(Enums.Resource Global)
        {
            return this.Get(3000000 + (int)Global);
        }

        internal void Set(int Global, int Count)
        {
            int i = this.FindIndex(R => R.Data == Global);

            if (i > -1)
                this[i].Count = Count;
            else
                this.Add(new Slot(Global, Count));
        }

        internal void Set(Enums.Resource Resource, int Count)
        {
            this.Set(3000000 + (int)Resource, Count);
        }

        internal void Plus(int Global, int Count)
        {
            int i = this.FindIndex(R => R.Data == Global);

            if (i > -1)
                this[i].Count += Count;
            else this.Add(new Slot(Global, Count));
        }

        internal void Plus(Enums.Resource Resource, int Count)
        {
            this.Plus(3000000 + (int)Resource, Count);
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

        internal void Minus(Enums.Resource _Resource, int _Value)
        {
            int Index = this.FindIndex(T => T.Data == 3000000 + (int)_Resource);

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

                Packet.AddInt(this.Count - 1);
                foreach (Slot Resource in this.Skip(1))
                {
                    Packet.AddInt(Resource.Data);
                    Packet.AddInt(Resource.Count);
                }

                return Packet.ToArray();
            }
        }

        internal void Initialize()
        {
            this.Set(Enums.Resource.Diamonds, 100000000);

            this.Set(Enums.Resource.Gold, 100000000);
            this.Set(Enums.Resource.Elixir, 100000000);
            this.Set(Enums.Resource.DarkElixir, 100000000);
            /*
            this.Set(Enums.Resource.Diamonds, (CSV.Tables.Get(Enums.Gamefile.Globals).GetData("STARTING_DIAMONDS") as Globals).NumberValue);

            this.Set(Enums.Resource.Gold, (CSV.Tables.Get(Enums.Gamefile.Globals).GetData("STARTING_GOLD") as Globals).NumberValue);
            this.Set(Enums.Resource.Elixir, (CSV.Tables.Get(Enums.Gamefile.Globals).GetData("STARTING_ELIXIR") as Globals).NumberValue);
#if DEBUG
            this.Set(Enums.Resource.DarkElixir, 100000000);
#endif
            */
        }
    }
}