using System.Collections.Generic;
using System.IO;
using Magic.ClashOfClans.Core;
using Magic.Files.Logic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    // Command 549
    internal class UpgradeMultipleBuildingsCommand : Command
    {
        public readonly List<int> GameObjectIds;
        public readonly bool IsAlternativeResource;

        public UpgradeMultipleBuildingsCommand(PacketReader br)
        {
            IsAlternativeResource = br.ReadBoolean();

            var count = br.ReadInt32WithEndian();
            GameObjectIds = new List<int>(count);
            for (var i = 0; i < count; i++)
            {
                var gameObjId = br.ReadInt32WithEndian();
                GameObjectIds.Add(gameObjId);
            }

            br.ReadInt32WithEndian();
        }

        public override void Execute(Level level)
        {
            var ca = level.Avatar;

            foreach (var buildingId in GameObjectIds)
            {
                var b = (Building)level.GameObjectManager.GetGameObjectByID(buildingId);
                if (b.CanUpgrade())
                {
                    var bd = b.BuildingData;
                    var cost = bd.GetBuildCost(b.GetUpgradeLevel() + 1);

                    ResourceData rd;

                    if (IsAlternativeResource == false)
                        rd = bd.GetBuildResource(b.GetUpgradeLevel() + 1);
                    else
                        rd = bd.GetAltBuildResource(b.GetUpgradeLevel() + 1);
                    if (ca.HasEnoughResources(rd, cost))
                    {
                        if (level.HasFreeWorkers())
                        {
                            string name = b.Data.Name;
                            Logger.Write("Building To Upgrade : " + name + " (" + buildingId + ')');
                            if (string.Equals(name, "Alliance Castle"))
                            {
                                ca.IncrementAllianceCastleLevel();
                                ca.SetAllianceCastleTotalCapacity(bd.GetUnitStorageCapacity(ca.GetAllianceCastleLevel()));
                            }
                            else if (string.Equals(name, "Town Hall"))
                                ca.IncrementTownHallLevel();

                            ca.SetResourceCount(rd, ca.GetResourceCount(rd) - cost);
                            b.StartUpgrading();
                        }
                    }
                }
            }
        }

    }
}