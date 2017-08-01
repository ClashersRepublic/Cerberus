using System.IO;
using Magic.Files.Logic;
using Savage.Magic;
using Savage.Magic.Network;

namespace Savage.Magic.Logic
{
    internal class UnitSlot
    {
        public int Count;
        public int Level;
        public CombatItemData UnitData;

        public UnitSlot(CombatItemData cd, int level, int count)
        {
            UnitData = cd;
            Level = level;
            Count = count;
        }

        public void Decode(PacketReader br)
        {
            UnitData = (CombatItemData) br.ReadDataReference();
            Level = br.ReadInt32();
            Count = br.ReadInt32();
        }
    }
}
