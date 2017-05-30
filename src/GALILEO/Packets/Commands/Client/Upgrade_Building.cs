using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.Servers.CoC.Core;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Files.CSV_Logic;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Logic.Structure;

namespace BL.Servers.CoC.Packets.Commands.Client
{
    internal class Upgrade_Building : Command
    {
        internal int BuildingId;
        internal uint Unknown1;
        internal bool IsAltResource;
        public Upgrade_Building(Reader reader, Device client, int id) : base(reader, client, id)
        {

        }

        internal override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            this.IsAltResource = this.Reader.ReadBoolean();
            this.Unknown1 = this.Reader.ReadUInt32();
        }

        internal override void Process()
        {
            var ca = this.Device.Player.Avatar;
            var go = this.Device.Player.Avatar.Variables.IsBuilderVillage ? this.Device.Player.GameObjectManager.GetBuilderVillageGameObjectByID(BuildingId) : this.Device.Player.GameObjectManager.GetGameObjectByID(BuildingId);
            if (go != null)
            {
                var b = (ConstructionItem)go;
                if (b.CanUpgrade())
                {
                    if (b.ClassId == 0 || b.ClassId == 7)
                    {
                        var bd = (Buildings) b.GetConstructionItemData();
                        Files.CSV_Logic.Resource rd = IsAltResource ? bd.GetAltBuildResource(b.GetUpgradeLevel() + 1)  : bd.GetBuildResource(b.GetUpgradeLevel() + 1);
                        if (ca.HasEnoughResources(rd.GetGlobalID(),
                            bd.GetBuildCost(b.GetUpgradeLevel())))
                        {
                            if (this.Device.Player.HasFreeWorkers)
                            {
                                var name = go.GetData().Row.Name;
#if DEBUG
                                Loggers.Log(b.ClassId == 0 ? "Building" : "Builder Building" + $" : Upgrading {name} with ID {BuildingId}", true);
#endif
                                if (bd.IsTownHall2())
                                {
                                    if (ca.Builder_TownHall_Level == 0)
                                    {
                                        Parallel.ForEach(this.Device.Player.GameObjectManager.GetGameObjects(7), Object =>
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

                                    ca.Builder_TownHall_Level++;
                                }
                               

                                if (bd.IsAllianceCastle())
                                {
                                    var a = (Building) go;
                                    var al = a.GetBuildingData;

                                    ca.Castle_Level++;
                                    ca.Castle_Total = al.GetUnitStorageCapacity(ca.Castle_Level);
                                    ca.Castle_Total_SP = al.GetAltUnitStorageCapacity(ca.Castle_Level);
                                }
                                else if (bd.IsTownHall())
                                    ca.TownHall_Level++;
                                
                                ca.Resources.Minus(rd.GetGlobalID(), bd.GetBuildCost(b.GetUpgradeLevel()));
                                b.StartUpgrading();
                            }
                        }
                    }
                    else if (b.ClassId == 4 || b.ClassId == 11)
                    {
                        var bd = (Traps)b.GetConstructionItemData();
                        if (ca.HasEnoughResources(bd.GetBuildResource(b.GetUpgradeLevel()).GetGlobalID(), bd.GetBuildCost(b.GetUpgradeLevel())))
                        {
                            if (this.Device.Player.HasFreeWorkers)
                            {
                                var name = go.GetData().Row.Name;
#if DEBUG
                                Loggers.Log($"Trap: Upgrading {name} with ID {BuildingId}", true);
#endif

                                var rd = bd.GetBuildResource(b.GetUpgradeLevel() + 1);
                                ca.Resources.Minus(rd.GetGlobalID(), bd.GetBuildCost(b.GetUpgradeLevel()));
                                b.StartUpgrading();
                            }
                        }
                    }
                    else if (b.ClassId == 8 || b.ClassId == 15)
                    {
                        var bd = (Village_Objects) b.GetConstructionItemData();
                        if (ca.HasEnoughResources(bd.GetBuildResource(b.GetUpgradeLevel()).GetGlobalID(),
                            bd.GetBuildCost(b.GetUpgradeLevel())))
                        {
                            if (this.Device.Player.HasFreeWorkers)
                            {
                                var name = go.GetData().Row.Name;
#if DEBUG
                                Loggers.Log($"Village Object: Upgrading {name} with ID {BuildingId}", true);
#endif

                                var rd = bd.GetBuildResource(b.GetUpgradeLevel() + 1);
                                ca.Resources.Minus(rd.GetGlobalID(), bd.GetBuildCost(b.GetUpgradeLevel()));
                                b.StartUpgrading();
                            }
                        }
                    }
                }
            }
        }
    }
}
