using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Boom.Database;
using CRepublic.Boom.Extensions;
using CRepublic.Boom.Extensions.List;
using CRepublic.Boom.Files;
using CRepublic.Boom.Files.CSV_Helpers;
using CRepublic.Boom.Logic.Enums;
using CRepublic.Boom.Logic.Slots.Items;

namespace CRepublic.Boom.Logic.Slots
{
   
    using System.Collections.Generic;
    using System.Linq;


    internal class Resources : List<Slot>
    {
        internal Avatar Player;

        /// <summary>
        /// Initializes a new instance of the <see cref="Resources"/> class.
        /// </summary>
        internal Resources()
        {
            // Resources.
        }

        internal Resources(Avatar _Player, bool Initialize = false)
        {
            this.Player = _Player;

            if (Initialize)
                this.Initialize();
        }

        internal int Gems => this.Get(Resource.Diamonds);

        internal int Get(int Gl_ID)
        {
            int i = this.FindIndex(R => R.Data == Gl_ID);

            if (i > -1)
                return this[i].Count;

            return 0;
        }

        internal int Get(Resource Gl_ID)
        {
            return this.Get(3000000 + (int) Gl_ID);
        }

        internal void Set(int Gl_ID, int Count)
        {
            int i = this.FindIndex(R => R.Data == Gl_ID);

            if (i > -1)
                this[i].Count = Count;
            else this.Add(new Slot(Gl_ID, Count));
        }

        internal void Set(Resource Resource, int Count)
        {
            this.Set(3000000 + (int) Resource, Count);
        }

        internal void Plus(int Gl_ID, int Count)
        {
            int i = this.FindIndex(R => R.Data == Gl_ID);

            if (i > -1)
                this[i].Count += Count;
            else this.Add(new Slot(Gl_ID, Count));
        }

        internal void Plus(Resource Resource, int Count)
        {
            this.Plus(3000000 + (int) Resource, Count);
        }

        internal bool Minus(int Gl_ID, int Count)
        {
            int i = this.FindIndex(R => R.Data == Gl_ID);

            if (i > -1)
                if (this[i].Count >= Count)
                {
                    this[i].Count -= Count;
                    return true;
                }

            return false;
        }

        internal void Minus(Resource _Resource, int _Value)
        {
            int Index = this.FindIndex(T => T.Data == 3000000 + (int) _Resource);

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
            this.Set(Resource.Diamonds, Utils.ParseConfigInt("startingGems"));

            this.Set(Resource.Resource1, Utils.ParseConfigInt("startingGold"));
            this.Set(Resource.Resource2, Utils.ParseConfigInt("startingWood"));
            this.Set(Resource.Resource3, Utils.ParseConfigInt("startingStone"));
#if DEBUG
            int Metal = Utils.ParseConfigInt("startingMetal");
            if (Metal != 0)
            this.Set(Resource.Resource4, Utils.ParseConfigInt("startingMetal")); //Metal
#endif
        }
    }
}