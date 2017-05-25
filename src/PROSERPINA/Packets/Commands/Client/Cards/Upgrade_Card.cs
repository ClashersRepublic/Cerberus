using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Commands.Client.Cards
{
    internal class Upgrade_Card : Command
    {
        internal int Tick = 0;
        internal int CardID = 0;
        internal int Type = 0;

        public Upgrade_Card(Reader _Reader, Device _Client, int _ID) : base(_Reader, _Client, _ID)
        {
            // Upgrade_Card.
        }

        internal override void Decode()
        {
            this.Tick = this.Reader.ReadVInt();
            this.Tick = this.Reader.ReadVInt();
            this.Reader.ReadInt16();

            this.Type = this.Reader.ReadVInt();
            this.CardID = this.Reader.ReadVInt();
        }

        internal override void Process()
        {
            int Index = this.Device.Player.Decks.FindIndex(_Card => _Card.Type == this.Type && _Card.ID == this.CardID + 1);

            Console.WriteLine(Index);

            if (Index > -1)
                this.Device.Player.Decks[Index].Upgrade();
        }
    }
}
