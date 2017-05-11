using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;
using BL.Servers.CR.Extensions.List;
using BL.Servers.CR.Logic.Slots;
using BL.Servers.CR.Logic.Slots.Items;

namespace BL.Servers.CR.Packets.Commands.Server.Chest
{
    internal class Buy_Chest_Callback : Command
    {
        internal int Tick = 0;
        internal int ChestID = 0;
        internal int Type = 4;
        internal int Gems = 1;
        internal int Gold = 1;

        internal List<Card> Cards = new List<Card>();

        public Buy_Chest_Callback(Device _Client) : base(_Client)
        {
            this.Identifier = 210;
        }

        List<int> _Cards = new List<int>(new int[]
        {
            1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,
            38,39,40,41,42,43,44,46,47,49,50,51,52,53,54,55,56,57,58,59,60,61
        });

        internal override void Encode()
        {
            Console.WriteLine("yo");
            this.Data.AddVInt(1);
            this.Data.AddVInt(0);
            switch (this.ChestID)
            {
                case 225:
                {
                    int Count = Core.Resources.Random.Next(10, 17);
                    this.Data.AddVInt(Count);

                    for (int i = 0; i < Count; i++)
                    {
                        int ID = _Cards[Core.Resources.Random.Next(0, this._Cards.Count)];

                        this.Data.AddVInt(ID); // Card ID
                        this.Data.AddVInt(0);
                        this.Data.AddVInt(0); // Tick
                        this.Data.AddVInt(Core.Resources.Random.Next(1, 100)); // Card Count
                        this.Data.AddVInt(0);
                        this.Data.AddVInt(0);
                        this.Data.AddVInt(0); // Is New  

                        this._Cards.Remove(ID);
                    }
                    break;
                }

                case 228:
                {
                    int Count = Core.Resources.Random.Next(10, 12);
                    this.Data.AddVInt(Count);

                    for (int i = 0; i < Count; i++)
                    {
                        int ID = _Cards[Core.Resources.Random.Next(0, this._Cards.Count)];

                        this.Data.AddVInt(ID); // Card ID
                        this.Data.AddVInt(0);
                        this.Data.AddVInt(0); // Tick
                        this.Data.AddVInt(Core.Resources.Random.Next(1, 50)); // Card Count
                        this.Data.AddVInt(0);
                        this.Data.AddVInt(0);
                        this.Data.AddVInt(0); // Is New   

                        this._Cards.Remove(ID);
                    }
                    break;
                }

                case 224:
                {
                    int Count = Core.Resources.Random.Next(6, 9);
                    this.Data.AddVInt(Count);

                    for (int i = 0; i < Count; i++)
                    {
                        int ID = _Cards[Core.Resources.Random.Next(0, this._Cards.Count)];

                        this.Data.AddVInt(ID); // Card ID
                        this.Data.AddVInt(0);
                        this.Data.AddVInt(0); // Tick
                        this.Data.AddVInt(Core.Resources.Random.Next(1, 20)); // Card Count
                        this.Data.AddVInt(0);
                        this.Data.AddVInt(0);
                        this.Data.AddVInt(0); // Is New    

                        this._Cards.Remove(ID);
                    }
                    break;
                }

                case 7:
                {
                    int Count = Core.Resources.Random.Next(2, 5);
                    this.Data.AddVInt(Count);

                    for (int i = 0; i < Count; i++)
                    {
                        int ID = _Cards[Core.Resources.Random.Next(0, this._Cards.Count)];

                        this.Data.AddVInt(ID); // Card ID
                        this.Data.AddVInt(0);
                        this.Data.AddVInt(0); // Tick
                        this.Data.AddVInt(Core.Resources.Random.Next(1, 10)); // Card Count
                        this.Data.AddVInt(0);
                        this.Data.AddVInt(0);
                        this.Data.AddVInt(0); // Is New    

                        this._Cards.Remove(ID);
                    }
                    break;
                }
                default:
                {
                    this.Data.AddVInt(_Cards.Count); // Card Count

                    for (int i = 0; i < _Cards.Count; i++)
                    {
                        int ID = _Cards[Core.Resources.Random.Next(0, this._Cards.Count)];

                        this.Data.AddVInt(ID); // Card ID
                        this.Data.AddVInt(0);
                        this.Data.AddVInt(0); // Tick
                        this.Data.AddVInt(Core.Resources.Random.Next(1, 1000)); // Card Count
                        this.Data.AddVInt(0);
                        this.Data.AddVInt(0);
                        this.Data.AddVInt(0); // Is New  

                        this._Cards.Remove(ID);
                    }
                    break;
                }
            }
            this.Data.AddHexa("7F");
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(this.Type);

            this.Data.AddHexa("7F7F0000");
            /*
            this.Data.AddVInt(1);

            this.Data.AddVInt(this.Cards.Count);

            foreach (Card _Card in this.Cards)
            {
                this.Data.AddVInt(_Card.ID);
                this.Data.AddVInt(_Card.Level);
                this.Data.AddVInt(0); // Chest Tick
                this.Data.AddVInt(_Card.Count);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(_Card.New);
            }

            this.Data.AddVInt(0);
            this.Data.AddVInt(this.Gems);

            this.Data.AddVInt(0);
            this.Data.AddVInt(this.Type);
            this.Data.AddVInt(this.ChestID);

            this.Data.Add(255);
            this.Data.Add(255);
            this.Data.Add(0);
            this.Data.Add(0);*/
        }
    }
}
