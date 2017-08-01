using System;
using System.Collections.Generic;
using System.IO;
using Magic.ClashOfClans.Core;
using Magic.Files.Logic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network
{
    internal class TrainUnitCommand : Command
    {
        public int Count;
        public int UnitType;
        public int Tick;

        public TrainUnitCommand(PacketReader br)
        {
            br.ReadInt32();
            int num1 = (int)br.ReadUInt32();
            UnitType = br.ReadInt32();
            Count = br.ReadInt32();
            int num2 = (int)br.ReadUInt32();
            Tick = br.ReadInt32();
        }

        public override void Execute(Level level)
        {
            Avatar avatar = level.Avatar;
            if (UnitType.ToString().StartsWith("400"))
            {
                CombatItemData _TroopData = (CombatItemData)CsvManager.DataTables.GetDataById(UnitType);
                List<DataSlot> units = level.Avatar.GetUnits();
                ResourceData trainingResource = _TroopData.GetTrainingResource();
                if (_TroopData == null)
                    return;
                DataSlot dataSlot1 = units.Find((Predicate<DataSlot>)(t => t.Data.GetGlobalID() == _TroopData.GetGlobalID()));
                if (dataSlot1 != null)
                {
                    dataSlot1.Value = dataSlot1.Value + Count;
                }
                else
                {
                    DataSlot dataSlot2 = new DataSlot((Data)_TroopData, Count);
                    units.Add(dataSlot2);
                }
                avatar.SetResourceCount(trainingResource, avatar.GetResourceCount(trainingResource) - _TroopData.GetTrainingCost(avatar.GetUnitUpgradeLevel(_TroopData)));
            }
            else
            {
                if (!this.UnitType.ToString().StartsWith("260"))
                    return;
                SpellData _SpellData = (SpellData)CsvManager.DataTables.GetDataById(UnitType);
                List<DataSlot> spells = level.Avatar.GetSpells();
                ResourceData trainingResource = _SpellData.GetTrainingResource();
                if (_SpellData == null)
                    return;
                DataSlot dataSlot1 = spells.Find((Predicate<DataSlot>)(t => t.Data.GetGlobalID() == _SpellData.GetGlobalID()));
                if (dataSlot1 != null)
                {
                    dataSlot1.Value = dataSlot1.Value + Count;
                }
                else
                {
                    DataSlot dataSlot2 = new DataSlot((Data)_SpellData, Count);
                    spells.Add(dataSlot2);
                }
                avatar.SetResourceCount(trainingResource, avatar.GetResourceCount(trainingResource) - _SpellData.GetTrainingCost(avatar.GetUnitUpgradeLevel((CombatItemData)_SpellData)));
            }
        }
    }
}
