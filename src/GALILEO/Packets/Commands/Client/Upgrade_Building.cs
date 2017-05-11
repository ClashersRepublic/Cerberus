using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Core;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Files.CSV_Logic;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Structure;
using NLog;

namespace BL.Servers.CoC.Packets.Commands.Client
{
    internal class Upgrade_Building : Command
    {
        internal int BuildingId;
        internal uint Unknown1;
        internal uint Unknown2;
        public Upgrade_Building(Reader reader, Device client, int id) : base(reader, client, id)
        {

        }

        internal override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            this.Unknown2 = this.Reader.ReadByte();
            this.Unknown1 = this.Reader.ReadUInt32();
        }

        internal override void Process()
        {
            var ca = this.Device.Player.Avatar;
            var go = this.Device.Player.GameObjectManager.GetGameObjectByID(BuildingId);
            if (go != null)
            {
                var b = (ConstructionItem)go;
                if (b.CanUpgrade())
                {
                    var bd = (Buildings) b.GetConstructionItemData();
                    if (ca.HasEnoughResources(bd.GetBuildResource(b.GetUpgradeLevel()).GetGlobalID(), bd.GetBuildCost(b.GetUpgradeLevel())))
                    {
                        if (this.Device.Player.HasFreeWorkers)
                        {
                            var name = this.Device.Player.GameObjectManager.GetGameObjectByID(BuildingId).GetData().Row.Name;
#if DEBUG
                            Loggers.Log($"Building: Upgrading {name} with ID {BuildingId}", true);
#endif
                            
                            if (bd.IsAllianceCastle())
                            {
                                var a = (Building)this.Device.Player.GameObjectManager.GetGameObjectByID(BuildingId);
                                var al = a.GetBuildingData();

                                ca.Castle_Level++;
                                ca.Castle_Total = al.GetUnitStorageCapacity(ca.Castle_Level);
                                ca.Castle_Total_SP = al.GetAltUnitStorageCapacity(ca.Castle_Level);
                            }
                            else if (bd.IsTownHall())
                                ca.TownHall_Level++;
                            var rd = bd.GetBuildResource(b.GetUpgradeLevel() + 1 );
                            ca.Resources.Minus(rd.GetGlobalID(), bd.GetBuildCost(b.GetUpgradeLevel()));
                            b.StartUpgrading();
                        }
                    }
                }
            }
        }
    }
}
