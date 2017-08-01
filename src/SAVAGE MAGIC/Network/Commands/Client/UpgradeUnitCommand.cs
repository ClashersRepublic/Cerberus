using System.IO;
using Magic.Files.Logic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    // Packet 516
    internal class UpgradeUnitCommand : Command
    {
        public UpgradeUnitCommand(PacketReader reader)
        {
            BuildingId = reader.ReadInt32();
            Unknown1 = reader.ReadUInt32();
            UnitData = (CombatItemData) reader.ReadDataReference();
            Unknown2 = reader.ReadUInt32();
        }

        public override void Execute(Level level)
        {
            var avatar = level.Avatar;
            var gameObject = level.GameObjectManager.GetGameObjectByID(BuildingId);
            var building = (Building) gameObject;
            var upgradeComponent = building.GetUnitUpgradeComponent();
            var unitLevel = avatar.GetUnitUpgradeLevel(UnitData);

            if (upgradeComponent.CanStartUpgrading(UnitData))
            {
                var cost = UnitData.GetUpgradeCost(unitLevel);
                var upgradeResource = UnitData.GetUpgradeResource(unitLevel);
                if (avatar.HasEnoughResources(upgradeResource, cost))
                {
                    avatar.SetResourceCount(upgradeResource, avatar.GetResourceCount(upgradeResource) - cost);
                    upgradeComponent.StartUpgrading(UnitData);
                }
            }
        }

        public int BuildingId { get; set; }
        public CombatItemData UnitData { get; set; }
        public uint Unknown1 { get; set; } 
        public uint Unknown2 { get; set; }
    }
}