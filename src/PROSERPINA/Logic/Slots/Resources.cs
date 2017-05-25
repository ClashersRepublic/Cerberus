using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Database;
using BL.Servers.CR.Extensions;
using BL.Servers.CR.Extensions.List;
using BL.Servers.CR.Files;
using BL.Servers.CR.Files.CSV_Helpers;
using BL.Servers.CR.Logic.Enums;
using BL.Servers.CR.Logic.Slots.Items;

namespace BL.Servers.CR.Logic.Slots
{
   
    using System.Collections.Generic;
    using System.Linq;


    internal class Resources : List<Items.Resource>
    {
        internal Player Player;

        /// <summary>
        /// Initializes a new instance of the <see cref="Resources"/> class.
        /// </summary>
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
            int i = this.FindIndex(R => R.Identifier == Gl_ID);

            if (i > -1)
                return this[i].Value;

            return 0;
        }

        internal int Get(Enums.Resource Gl_ID)
        {
            return this.Get(3000000 + (int) Gl_ID);
        }

        internal void Set(int Gl_ID, int Count)
        {
            int i = this.FindIndex(R => R.Identifier == Gl_ID);

            if (i > -1)
                this[i].Value = Count;
            else this.Add(new Items.Resource(Gl_ID, Count));
        }

        internal void Set(Enums.Resource Resource, int Count)
        {
            this.Set(3000000 + (int)Resource, Count);
        }

        internal void Plus(int Gl_ID, int Count)
        {
            int i = this.FindIndex(R => R.Identifier == Gl_ID);

            if (i > -1)
                this[i].Value += Count;
            else this.Add(new Items.Resource(Gl_ID, Count));
        }

        internal void Plus(Enums.Resource Resource, int Count)
        {
            this.Plus(3000000 + (int) Resource, Count);
        }

        internal bool Minus(int Gl_ID, int Count)
        {
            int i = this.FindIndex(R => R.Identifier == Gl_ID);

            if (i > -1)
                if (this[i].Value >= Count)
                {
                    this[i].Value -= Count;
                    return true;
                }

            return false;
        }

        internal void Minus(Enums.Resource _Resource, int _Value)
        {
            int Index = this.FindIndex(T => T.Identifier == 3000000 + (int) _Resource);

            if (Index > -1)
            {
                this[Index].Value -= _Value;
            }
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> Packet = new List<byte>();

                Packet.AddInt(this.Count - 1);
                foreach (Items.Resource Resource in this.Skip(1))
                {
                    Packet.AddInt(Resource.Identifier);
                    Packet.AddInt(Resource.Value);
                }

                return Packet.ToArray();
            }
        }

        internal void Initialize()
        {
            this.Set(Enums.Resource.Diamonds, Utils.ParseConfigInt("startingGems"));
            this.Set(Enums.Resource.Gold, Utils.ParseConfigInt("startingGold"));
            this.Set(Enums.Resource.Chest_Index, 0);
            this.Set(Enums.Resource.Chest_Count, 0);
            this.Set(Enums.Resource.Free_Gold, 0);
            this.Set(Enums.Resource.Max_Trophies, 0);
            this.Set(Enums.Resource.Card_Count, 0);
            this.Set(Enums.Resource.Donations, 0);
            this.Set(Enums.Resource.Reward_Gold, 0);
            this.Set(Enums.Resource.Reward_Count, 0);
            this.Set(Enums.Resource.Shop_Day_Count, 0);
        }
    }
}