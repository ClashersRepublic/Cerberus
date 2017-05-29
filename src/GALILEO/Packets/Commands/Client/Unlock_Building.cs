using BL.Servers.CoC.Core;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Files.CSV_Logic;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Logic.Structure;

namespace BL.Servers.CoC.Packets.Commands.Client
{

    internal class Unlock_Building : Command
    {
        internal int BuildingId;
        internal uint Unknown1;

        public Unlock_Building(Reader reader, Device client, int id) : base(reader, client, id)
        {

        }

        internal override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            this.Unknown1 = this.Reader.ReadUInt32();
        }

        internal override void Process()
        {
            var ca = this.Device.Player.Avatar;

            var go = this.Device.Player.Avatar.Variables.IsBuilderVillage ? this.Device.Player.GameObjectManager.GetBuilderVillageGameObjectByID(BuildingId) : this.Device.Player.GameObjectManager.GetGameObjectByID(BuildingId);

            var b = (ConstructionItem) go;

            var bd = (Buildings) b.GetConstructionItemData();

            if (ca.HasEnoughResources(bd.GetBuildResource(b.GetUpgradeLevel()).GetGlobalID(), bd.GetBuildCost(b.GetUpgradeLevel())))
            {
                var name = go.GetData().Row.Name;
#if DEBUG
                Loggers.Log($"Building: Unlocking {name} with ID {BuildingId}", true);
#endif
                if (bd.IsAllianceCastle())
                {
                    var a = (Building)go;
                    var al = a.GetBuildingData;

                    ca.Castle_Level++;
                    ca.Castle_Total = al.GetUnitStorageCapacity(ca.Castle_Level);
                    ca.Castle_Total_SP = al.GetAltUnitStorageCapacity(ca.Castle_Level);
                }

                var rd = bd.GetBuildResource(b.GetUpgradeLevel());
                ca.Resources.Minus(rd.GetGlobalID(), bd.GetBuildCost(b.GetUpgradeLevel()));
                b.Unlock();
            }
        }
    }
}