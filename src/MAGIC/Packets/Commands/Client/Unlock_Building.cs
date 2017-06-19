using CRepublic.Magic.Core;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Files;
using CRepublic.Magic.Files.CSV_Logic;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Logic.Structure;

namespace CRepublic.Magic.Packets.Commands.Client
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
                if (name == "Hero Altar Warmachine")
                {

                    if (b.GetHeroBaseComponent(true) != null)
                    {
                        Buildings data = (Buildings)b. GetData();
                        Heroes hd = CSV.Tables.Get(Gamefile.Heroes).GetData(data.HeroType) as Heroes;
                        this.Device.Player.Avatar.SetUnitUpgradeLevel(hd, 0);
                        this.Device.Player.Avatar.SetHeroHealth(hd, 0);
                        this.Device.Player.Avatar.SetHeroState(hd, 3);
                    }
                }

                var rd = bd.GetBuildResource(b.GetUpgradeLevel());
                ca.Resources.Minus(rd.GetGlobalID(), bd.GetBuildCost(b.GetUpgradeLevel()));
                b.Unlock();
            }
        }
    }
}