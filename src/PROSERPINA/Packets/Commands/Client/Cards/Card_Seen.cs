using System;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Commands.Client.Cards
{
    internal class Card_Seen : Command
    {
        internal int Tick = 0;

        internal int Type = 0;
        internal int CardID = 0;

        public Card_Seen(Reader _Reader, Device _Client, int _ID) : base(_Reader, _Client, _ID)
        {
            // Card_Seen.
        }
        internal override void Decode()
        {
            this.Tick = this.Reader.ReadVInt();
            this.Tick = this.Reader.ReadVInt();
            this.Reader.ReadInt16();

            this.Reader.ReadVInt();
            this.Type = this.Reader.ReadVInt();
            this.CardID = this.Reader.ReadVInt();
        }

        internal override void Process()
        {
            this.ShowValues();

            int Index = this.Device.Player.Avatar.Decks.FindIndex(_Card => _Card.Type == this.Type && _Card.ID == this.CardID + 1);

            if (Index > -1)
            this.Device.Player.Avatar.Decks[Index].New = 0;
        }
    }
}
