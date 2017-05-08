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

        internal Decks Cards = new Decks();

        public Buy_Chest_Callback(Reader _Reader, Device _Client, int _ID) : base(_Reader, _Client, _ID)
        {
        }

        public Buy_Chest_Callback(Device _Client) : base(_Client)
        {
            this.Identifier = 210;
        }

        internal override void Decode()
        {
            this.Reader.ReadBytes((int) this.Reader.BaseStream.Length - 12);

            this.Tick = this.Reader.ReadRRInt32();
            this.Tick = this.Reader.ReadRRInt32();

            this.Reader.ReadInt16();
        }

        internal override void Encode()
        {
            this.Data.AddVInt(1);

            this.Data.AddVInt(this.Cards.Count);

            foreach (Card _Card in this.Cards)
            {
                this.Data.AddVInt(_Card.Type);
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
            this.Data.Add(0);
        }
    }
}
