using System;
using System.Collections.Generic;
using System.Linq;
using CRepublic.Royale.Extensions.List;
using CRepublic.Royale.Logic.Slots.Items;
using Newtonsoft.Json;

namespace CRepublic.Royale.Logic.Slots
{
    internal class Decks : List<Card>
    {
        internal Player Player;
        
        public Decks()
        {
            
        }

        public Decks(Player _Player)
        {
            this.Player = _Player;
        }

        public new void Add(Card _Card)
        {
            if (this.Contains(_Card))
            {
                int _Index = this.FindIndex(Card => Card.ID == _Card.ID && Card.Type == _Card.Type);

                if (_Index > -1)
                {
                    this[_Index].Count += _Card.Count;
                }
                else
                {
                    base.Add(_Card);
                }
            }
            else
            {
                base.Add(_Card);
            }
        }

        public void Add(byte _Type, int _ID, int _Count, int _Level, byte _isNew)
        {
            Card _Card = new Card(_Type, _ID, _Count, _Level, _isNew);

            if (this.Contains(_Card))
            {
                int _Index = this.FindIndex(Card => Card.ID == _Card.ID && Card.Type == _Card.Type);

                if (_Index > -1)
                {
                    this[_Index].Count += _Card.Count;
                }
                else
                {
                    base.Add(_Card);
                }
            }
            else
            {
                base.Add(_Card);
            }
        }

        public void Invert(int _ID, int _Position)
        {
            Card _Old = this[_Position];
            this[_Position] = this[_ID + 8];
            this[_ID + 8] = _Old;
        }

        public byte[] ToBytes()
        {
            List<byte> _Packet = new List<byte>();

            foreach (Card _Card in this.GetRange(0, 8))
            {
                _Packet.AddVInt(_Card.Type == 28 ? _Card.ID + 64 : _Card.ID); // Card ID
                _Packet.AddVInt(_Card.Level); // Card Level
                _Packet.AddVInt(0); // Unknown
                _Packet.AddVInt(_Card.Count); // Card Count
                _Packet.AddVInt(0); // Unknown
                _Packet.AddVInt(0); // Unknown
                _Packet.AddVInt(_Card.New); // New Card = 2
            }

            return _Packet.ToArray();
        }

        public byte[] Hand()
        {
            List<byte> _Packet = new List<byte>();

            foreach (Card _Card in this.GetRange(0, 8))
            {
                _Packet.AddVInt(_Card.Type == 28 ? _Card.ID + 64 : _Card.ID); // Card ID
                _Packet.AddVInt(_Card.Level); // Card Level
            }

            return _Packet.ToArray();
        }

        public void Die()
        {
            Console.WriteLine(JsonConvert.SerializeObject(this, Core.Server_Resources.Players.Settings));
        }
    }
}
