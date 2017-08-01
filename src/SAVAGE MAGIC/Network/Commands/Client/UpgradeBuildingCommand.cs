using System.IO;
using Magic.ClashOfClans.Core;
using Magic.Files.Logic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class UpgradeBuildingCommand : Command
    {
        public int BuildingId;
        public uint Unknown1;
        public uint Unknown2;

        public UpgradeBuildingCommand(PacketReader br)
        {
            BuildingId = br.ReadInt32();
            Unknown2 = br.ReadByte();
            Unknown1 = br.ReadUInt32();
        }

        public override void Execute(Level level)
        {
            Avatar avatar = level.Avatar;
            GameObject gameObjectById = level.GameObjectManager.GetGameObjectByID(BuildingId);
            if (gameObjectById == null)
                return;

            ConstructionItem constructionItem = (ConstructionItem)gameObjectById;
            if (!constructionItem.CanUpgrade())
                return;

            ConstructionItemData constructionItemData = constructionItem.GetConstructionItemData();
            if (!avatar.HasEnoughResources(constructionItemData.GetBuildResource(constructionItem.GetUpgradeLevel() + 1), constructionItemData.GetBuildCost(constructionItem.GetUpgradeLevel() + 1)) || !level.HasFreeWorkers())
                return;

            string name = level.GameObjectManager.GetGameObjectByID(BuildingId).Data.Name;
            Logger.Write("Building To Upgrade : " + name + " (" + BuildingId + ')');
            if (string.Equals(name, "Alliance Castle"))
            {
                avatar.IncrementAllianceCastleLevel();
                BuildingData buildingData = ((Building)level.GameObjectManager.GetGameObjectByID(BuildingId)).BuildingData;
                avatar.SetAllianceCastleTotalCapacity(buildingData.GetUnitStorageCapacity(avatar.GetAllianceCastleLevel()));
            }
            else if (string.Equals(name, "Town Hall"))
                avatar.IncrementTownHallLevel();

            ResourceData buildResource = constructionItemData.GetBuildResource(constructionItem.GetUpgradeLevel() + 1);
            avatar.SetResourceCount(buildResource, avatar.GetResourceCount(buildResource) - constructionItemData.GetBuildCost(constructionItem.GetUpgradeLevel() + 1));
            constructionItem.StartUpgrading();
        }
    }
}