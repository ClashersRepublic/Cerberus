using System.IO;
using Magic.ClashOfClans.Core;
using Magic.Files.Logic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class CancelUnitProductionCommand : Command
    {
        public int BuildingId;
        public int Count;
        public int UnitType;
        public uint Unknown1;
        public uint Unknown3;
        public uint Unknown4;

        public CancelUnitProductionCommand(PacketReader br)
        {
            BuildingId = br.ReadInt32(); 
            Unknown1 = br.ReadUInt32();
            UnitType = br.ReadInt32();
            Count = br.ReadInt32();
            Unknown3 = br.ReadUInt32();
            Unknown4 = br.ReadUInt32();
        }

        public override void Execute(Level level)
        {
            // No need for cancelling of unit production.

            //GameObject gameObjectById = level.GameObjectManager.GetGameObjectByID(BuildingId);
            //if (Count <= 0)
            //  return;

            //UnitProductionComponent productionComponent = ((ConstructionItem)gameObjectById).GetUnitProductionComponent(false);
            //CombatItemData dataById = (CombatItemData)CSVManager.DataTables.GetDataById(UnitType);
            //do
            //{
            //    productionComponent.RemoveUnit(dataById);
            //    Count = Count - 1;
            //}
            //while (Count > 0);
        }
    }
}
