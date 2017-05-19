using System.Collections.Generic;
using BL.Servers.CoC.Core;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Files.CSV_Logic;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Structure;
using NLog;

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
                var b = (Building) this.Device.Player.GameObjectManager.GetGameObjectByID(buildingId);
                if (b.CanUpgrade())
                {
                    var bd = b.GetBuildingData;
                    var cost = bd.GetBuildCost(b.GetUpgradeLevel() + 1);
                    Resource rd = IsAltResource ? bd.GetAltBuildResource(b.GetUpgradeLevel() + 1) : bd.GetBuildResource(b.GetUpgradeLevel() + 1);
                    if (Avatar.HasEnoughResources(rd.GetGlobalID(), cost))
                    {
                        if (this.Device.Player.HasFreeWorkers)
                        {
                            var name = this.Device.Player.GameObjectManager.GetGameObjectByID(buildingId).GetData().Row
                                .Name;
#if DEBUG
                            Loggers.Log($"Building: Upgrading {name} with ID {buildingId}", true);
#endif

                            if (bd.IsAllianceCastle())
                            {
                                var a = (Building) this.Device.Player.GameObjectManager.GetGameObjectByID(buildingId);
                                var al = a.GetBuildingData;

                                Avatar.Castle_Level++;
                                Avatar.Castle_Total = al.GetUnitStorageCapacity(Avatar.Castle_Level);
                                Avatar.Castle_Total_SP = al.GetAltUnitStorageCapacity(Avatar.Castle_Level);
                            }
                            else if (bd.IsTownHall())
                                Avatar.TownHall_Level++;
                            Avatar.Resources.Minus(rd.GetGlobalID(), cost);
                            b.StartUpgrading();
                        }
                    }
                }
            }
        }
    }
}
