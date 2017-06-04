using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Files;
using BL.Servers.CoC.Files.CSV_Logic;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Logic.Structure;

namespace BL.Servers.CoC.Packets.Commands.Client
{
    internal class Upgrade_Hero : Command
    {
        internal int BuildingId;
        internal uint Tick;
        public Upgrade_Hero(Reader reader, Device client, int id) : base(reader, client, id)
        {

        }

        internal override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            this.Tick = this.Reader.ReadUInt32();
        }

        internal override void Process()
        {
            var ca = this.Device.Player.Avatar;
            var go = this.Device.Player.Avatar.Variables.IsBuilderVillage ? this.Device.Player.GameObjectManager.GetBuilderVillageGameObjectByID(BuildingId) : this.Device.Player.GameObjectManager.GetGameObjectByID(BuildingId);
            if (go != null)
            {
                var hbc = this.Device.Player.Avatar.Variables.IsBuilderVillage ? (go as Builder_Building).GetHeroBaseComponent() : (go as Building).GetHeroBaseComponent();
                if (hbc != null)
                {
                    if (hbc.CanStartUpgrading())
                    {
                        var hd = (CSV.Tables.Get(Gamefile.Heroes).GetData(hbc.HeroData.Name) as Heroes);
                        var currentLevel = ca.GetUnitUpgradeLevel(hd);
                        var rd = hd.GetUpgradeResource();
                        var cost = hd.GetUpgradeCost(currentLevel);
                        if (ca.HasEnoughResources(rd.GetGlobalID(), cost))
                        {
                            if (this.Device.Player.Avatar.Variables.IsBuilderVillage ? this.Device.Player.HasFreeBuilderVillageWorkers : this.Device.Player.HasFreeVillageWorkers)
                            {
                                hbc.StartUpgrading(this.Device.Player.Avatar.Variables.IsBuilderVillage);
                            }
                        }
                    }
                }
            }
        }
    }
}
