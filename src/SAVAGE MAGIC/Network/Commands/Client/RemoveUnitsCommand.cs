using System;
using System.Collections.Generic;
using System.IO;
using Magic.ClashOfClans.Core;
using Magic.Files.Logic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.Packets.Commands.Client;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class RemoveUnitsCommand : Command
    {
        public List<UnitToRemove> UnitsToRemove;
        public int UnitTypesCount;

        public RemoveUnitsCommand(PacketReader br)
        {
            int num1 = (int) br.ReadUInt32();
            UnitTypesCount = br.ReadInt32();
            UnitsToRemove = new List<UnitToRemove>();

            for (var i = 0; i < UnitTypesCount; i++)
            {
                int num2 = br.ReadInt32();
                int num3 = br.ReadInt32();
                int num4 = br.ReadInt32();
                UnitsToRemove.Add(new UnitToRemove()
                {
                    Data = num2,
                    Count = num3,
                    Level = num4
                });
            }

            int num5 = (int) br.ReadUInt32();
        }

        public override void Execute(Level level)
        {
            List<DataSlot> units = level.Avatar.GetUnits();
            List<DataSlot> spells = level.Avatar.GetSpells();
            foreach (UnitToRemove unitToRemove in UnitsToRemove)
            {
                if (unitToRemove.Data.ToString().StartsWith("400"))
                {
                    CombatItemData _Troop = (CombatItemData) CsvManager.DataTables.GetDataById(unitToRemove.Data);
                    DataSlot dataSlot = units.Find((Predicate<DataSlot>)(t => t.Data.GetGlobalID() == _Troop.GetGlobalID()));
                    if (dataSlot != null)
                        dataSlot.Value = dataSlot.Value - 1;
                }
                else if (unitToRemove.Data.ToString().StartsWith("260"))
                {
                    SpellData _Spell = (SpellData)CsvManager.DataTables.GetDataById(unitToRemove.Data);
                    DataSlot dataSlot = spells.Find((Predicate<DataSlot>)(t => t.Data.GetGlobalID() == _Spell.GetGlobalID()));
                    if (dataSlot != null)
                        dataSlot.Value = dataSlot.Value - 1;
                }
            }
        }
    }
}
