using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Servers.CoC.Core;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Files.CSV_Logic;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Structure;
using Resource = BL.Servers.CoC.Files.CSV_Logic.Resource;

namespace BL.Servers.CoC.Packets.Commands.Client
{
    internal class Upgrade_Multiple_Buildings : Command
    {

        internal List<int> BuildingIds;
        internal bool IsAltResource;
        internal int Tick;

        public Upgrade_Multiple_Buildings(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.IsAltResource = this.Reader.ReadBoolean();
            int buildingCount = this.Reader.ReadInt32();
            this.BuildingIds = new List<int>(buildingCount);
            for (var i = 0; i < buildingCount; i++)
            {
                var buildingId = this.Reader.ReadInt32();
                this.BuildingIds.Add(buildingId);
            }
            this.Tick = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            var Avatar = this.Device.Player.Avatar;

            foreach (var buildingId in this.BuildingIds)
            {
                var go = this.Device.Player.Avatar.Variables.IsBuilderVillage
                    ? this.Device.Player.GameObjectManager.GetBuilderVillageGameObjectByID(buildingId)
                    : this.Device.Player.GameObjectManager.GetGameObjectByID(buildingId);
                if (go != null)
                {

                    var b = (ConstructionItem) go;
                    if (b.CanUpgrade())
                    {
                        if (b.ClassId == 0 || b.ClassId == 7)
                        {
                            var bd = (Buildings) b.GetConstructionItemData();
                            var cost = bd.GetBuildCost(b.GetUpgradeLevel() + 1);
                            Resource rd = IsAltResource
                                ? bd.GetAltBuildResource(b.GetUpgradeLevel() + 1)
                                : bd.GetBuildResource(b.GetUpgradeLevel() + 1);
                            if (Avatar.HasEnoughResources(rd.GetGlobalID(), cost))
                            {
                                if (this.Device.Player.Avatar.Variables.IsBuilderVillage
                                    ? this.Device.Player.HasFreeBuilderVillageWorkers
                                    : this.Device.Player.HasFreeVillageWorkers)
                                {
#if DEBUG
                                    Loggers.Log($"Building: Upgrading {go.GetData().Row.Name} with ID {buildingId}", true);
#endif
                                    if (bd.IsTownHall2())
                                    {
                                        if (Avatar.Builder_TownHall_Level == 0)
                                        {
                                            Parallel.ForEach(this.Device.Player.GameObjectManager.GetGameObjects(7),
                                                Object =>
                                                {
                                                    Builder_Building b2 = (Builder_Building) Object;
                                                    var bd2 = b2.GetBuildingData;
                                                    if (b2.Locked)
                                                    {
                                                        if (bd2.Locked)
                                                            return;
#if DEBUG
                                                        Loggers.Log(
                                                            $"Builder Building: Unlocking {bd2.Name} with ID {Object.GlobalId}",
                                                            true);
#endif
                                                        b2.Unlock();
                                                    }
                                                });

                                        }
                                        Avatar.Builder_TownHall_Level++;
                                    }

                                    if (bd.IsAllianceCastle())
                                    {
                                        var a = (Building) this.Device.Player.GameObjectManager
                                            .GetGameObjectByID(buildingId);
                                        var al = a.GetBuildingData;

                                        Avatar.Castle_Level++;
                                        Avatar.Castle_Total = al.GetUnitStorageCapacity(Avatar.Castle_Level);
                                        Avatar.Castle_Total_SP = al.GetAltUnitStorageCapacity(Avatar.Castle_Level);
                                    }
                                    else if (bd.IsTownHall())
                                        Avatar.TownHall_Level++;

                                    Avatar.Resources.Minus(rd.GetGlobalID(), cost);
                                    b.StartUpgrading(this.Device.Player.Avatar.Variables.IsBuilderVillage);
                                }
                            }
                        }
                        else if (b.ClassId == 4 || b.ClassId == 11)
                        {
                            var bd = (Traps) b.GetConstructionItemData();
                            if (Avatar.HasEnoughResources(bd.GetBuildResource(b.GetUpgradeLevel()).GetGlobalID(),
                                bd.GetBuildCost(b.GetUpgradeLevel())))
                            {
                                if (this.Device.Player.Avatar.Variables.IsBuilderVillage
                                    ? this.Device.Player.HasFreeBuilderVillageWorkers
                                    : this.Device.Player.HasFreeVillageWorkers)
                                {
                                    var name = go.GetData().Row.Name;
#if DEBUG
                                    Loggers.Log($"Trap: Upgrading {name} with ID {buildingId}", true);
#endif

                                    var rd = bd.GetBuildResource(b.GetUpgradeLevel() + 1);
                                    Avatar.Resources.Minus(rd.GetGlobalID(), bd.GetBuildCost(b.GetUpgradeLevel()));
                                    b.StartUpgrading(this.Device.Player.Avatar.Variables.IsBuilderVillage);
                                }
                            }
                        }
                        else if (b.ClassId == 8 || b.ClassId == 15)
                        {
                            var bd = (Village_Objects) b.GetConstructionItemData();
                            if (Avatar.HasEnoughResources(bd.GetBuildResource(b.GetUpgradeLevel()).GetGlobalID(),
                                bd.GetBuildCost(b.GetUpgradeLevel())))
                            {
                                if (this.Device.Player.Avatar.Variables.IsBuilderVillage
                                    ? this.Device.Player.HasFreeBuilderVillageWorkers
                                    : this.Device.Player.HasFreeVillageWorkers)
                                {
                                    var name = go.GetData().Row.Name;
#if DEBUG
                                    Loggers.Log($"Village Object: Upgrading {name} with ID {buildingId}", true);
#endif

                                    var rd = bd.GetBuildResource(b.GetUpgradeLevel() + 1);
                                    Avatar.Resources.Minus(rd.GetGlobalID(), bd.GetBuildCost(b.GetUpgradeLevel()));
                                    b.StartUpgrading(this.Device.Player.Avatar.Variables.IsBuilderVillage);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
