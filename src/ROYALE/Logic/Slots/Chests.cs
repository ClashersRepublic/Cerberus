using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Extensions.List;
using CRepublic.Royale.Logic.Slots.Items;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CRepublic.Royale.Logic.Slots
{
    internal class Chests : List<Chest>
    {
        public new void Add(byte _Emplacement, int _ID, bool _Unlocked, DateTime _UnlockTime, bool _isNew)
        {
            Chest _Chest = new Chest(_Emplacement, _ID, _Unlocked, _UnlockTime, _isNew);

            if (this.Contains(_Chest))
            {
                int _Index = this.FindIndex(chest => chest == _Chest);

                if (_Index > -1)
                {
                    this[_Index].Unlocked = _Unlocked;
                    this[_Index].UnlockTime = _UnlockTime;
                }
                else
                {
                    base.Add(_Chest);
                }
            }
            else
            {
                base.Add(_Chest);
            }
        }

        public byte[] ToBytes()
        {
            int TimeStamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

            List<byte> _Packet = new List<byte>();

            _Packet.AddVInt(this.Count > 0 ? 1 : 0);

            foreach (Chest _Chest in this.OrderBy(_Chest => _Chest.Emplacement))
            {
                _Packet.Add(13);
                _Packet.AddVInt(_Chest.ID);
                _Packet.AddVInt(_Chest.Unlocked ? 1 : 0);
                _Packet.AddVInt((int)(_Chest.UnlockTime - DateTime.UtcNow).TotalSeconds);
                _Packet.Add(133);
                _Packet.Add(61);
                _Packet.Add(1);
                _Packet.AddVInt(_Chest.Emplacement);
                _Packet.Add(0);
                _Packet.AddVInt(2);
            }

            return _Packet.ToArray();
        }
    }
}
