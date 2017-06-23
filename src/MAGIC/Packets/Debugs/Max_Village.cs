using System.Text;
using System.Threading.Tasks;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Files;
using CRepublic.Magic.Files.CSV_Logic;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Logic.Structure;
using CRepublic.Magic.Packets.Messages.Server;

namespace CRepublic.Magic.Packets.Debugs
{
    internal class Max_Village : Debug
    {
        internal int VillageID;
        internal StringBuilder Help;

        public Max_Village(Device device, params string[] Parameters) : base(device, Parameters)
        {

        }

        internal override void Process()
        {
            if (this.Parameters.Length >= 1)
            {
                if (int.TryParse(this.Parameters[0], out this.VillageID))
                {
                    switch (this.VillageID)
                    {
                        case 0:

                            Parallel.ForEach(this.Device.Player.GameObjectManager.GetGameObjects(0), b =>
                            {
                                var building = (ConstructionItem)b;
                                var data = (Buildings)building.GetConstructionItemData();

                                if (building.Locked)
                                    building.Unlock();

                                if (building.IsConstructing)
                                    building.IsConstructing = false;

                                if (data.IsTownHall())
                                    this.Device.Player.Avatar.TownHall_Level = data.GetUpgradeLevelCount() - 1;

                                if (data.IsAllianceCastle())
                                {
                                    var al = ((Building)b).GetBuildingData;
                                    this.Device.Player.Avatar.Castle_Level = data.GetUpgradeLevelCount() - 1;
                                    this.Device.Player.Avatar.Castle_Total = al.GetUnitStorageCapacity(this.Device.Player.Avatar.Castle_Level);
                                    this.Device.Player.Avatar.Castle_Total_SP = al.GetAltUnitStorageCapacity(this.Device.Player.Avatar.Castle_Level);
                                }

                                building.SetUpgradeLevel(data.GetUpgradeLevelCount() - 1);
                            });

                            Parallel.ForEach(this.Device.Player.GameObjectManager.GetGameObjects(4), t =>
                            {
                                var trap = (Trap) t;
                                var data = (Traps) trap.Data;

                                trap.SetUpgradeLevel(data.GetUpgradeLevelCount() - 1);
                            });

                            SendChatMessage("Command Processor: Success, Enjoy!");
                            new Own_Home_Data(this.Device).Send();

                            break;

                        case 1:
                            if (this.Device.Player.Avatar.Builder_TownHall_Level > 0)
                            {
                                Parallel.ForEach(this.Device.Player.GameObjectManager.GetGameObjects(7), b =>
                                {
                                    var building = (ConstructionItem) b;
                                    var data = (Buildings) building.GetConstructionItemData();

                                    if (building.Locked)
                                    {
                                        building.Unlock();
                                        if (data.Name == "Hero Altar Warmachine")
                                        {
                                            if (building.GetHeroBaseComponent(true) != null)
                                            {
                                                Heroes hd = CSV.Tables.Get(Gamefile.Heroes)
                                                    .GetData(data.HeroType) as Heroes;
                                                this.Device.Player.Avatar.SetUnitUpgradeLevel(hd, 0);
                                                this.Device.Player.Avatar.SetHeroHealth(hd, 0);
                                                this.Device.Player.Avatar.SetHeroState(hd, 3);
                                            }
                                        }
                                    }

                                    if (building.IsConstructing)
                                        building.IsConstructing = false;

                                    if (data.IsTownHall2())
                                        this.Device.Player.Avatar.Builder_TownHall_Level =
                                            data.GetUpgradeLevelCount() - 1;
                                    building.SetUpgradeLevel(data.GetUpgradeLevelCount() - 1);
                                });

                                Parallel.ForEach(this.Device.Player.GameObjectManager.GetGameObjects(11), t =>
                                {
                                    var trap = (Builder_Trap) t;
                                    var data = (Traps) trap.Data;

                                    trap.SetUpgradeLevel(data.GetUpgradeLevelCount() - 1);
                                });

                                SendChatMessage("Command Processor: Success, Enjoy!");
                                new Own_Home_Data(this.Device).Send();
                            }
                            else
                                SendChatMessage("Command Processor: Please visit builder village first before running this mode!");
                            break;

                        case 2:
                            if (this.Device.Player.Avatar.Builder_TownHall_Level > 0)
                            {
                                Parallel.ForEach(this.Device.Player.GameObjectManager.GetGameObjects(0), b =>
                                {
                                    var building = (Building) b;
                                    var data = (Buildings) building.Data;

                                    if (building.Locked)
                                        building.Unlock();

                                    if (building.IsConstructing)
                                        building.IsConstructing = false;

                                    if (data.IsTownHall())
                                        this.Device.Player.Avatar.TownHall_Level = data.GetUpgradeLevelCount() - 1;

                                    if (data.IsAllianceCastle())
                                    {
                                        var al = ((Building)b).GetBuildingData;
                                        this.Device.Player.Avatar.Castle_Level = data.GetUpgradeLevelCount() - 1;
                                        this.Device.Player.Avatar.Castle_Total = al.GetUnitStorageCapacity(this.Device.Player.Avatar.Castle_Level);
                                        this.Device.Player.Avatar.Castle_Total_SP = al.GetAltUnitStorageCapacity(this.Device.Player.Avatar.Castle_Level);
                                    }


                                    building.SetUpgradeLevel(data.GetUpgradeLevelCount() - 1);
                                });

                                Parallel.ForEach(this.Device.Player.GameObjectManager.GetGameObjects(4), t =>
                                {
                                    var trap = (Trap) t;
                                    var data = (Traps) trap.Data;

                                    trap.SetUpgradeLevel(data.GetUpgradeLevelCount() - 1);
                                });

                                Parallel.ForEach(this.Device.Player.GameObjectManager.GetGameObjects(7), b =>
                                {
                                    var building = (ConstructionItem) b;
                                    var data = (Buildings) building.GetConstructionItemData();


                                    if (building.Locked)
                                    {
                                        building.Unlock();
                                        if (data.Name == "Hero Altar Warmachine")
                                        {
                                            if (building.GetHeroBaseComponent(true) != null)
                                            {
                                                Heroes hd = CSV.Tables.Get(Gamefile.Heroes)
                                                    .GetData(data.HeroType) as Heroes;
                                                this.Device.Player.Avatar.SetUnitUpgradeLevel(hd, 0);
                                                this.Device.Player.Avatar.SetHeroHealth(hd, 0);
                                                this.Device.Player.Avatar.SetHeroState(hd, 3);
                                            }
                                        }
                                    }

                                    if (building.IsConstructing)
                                        building.IsConstructing = false;

                                    if (data.IsTownHall2())
                                        this.Device.Player.Avatar.Builder_TownHall_Level =
                                            data.GetUpgradeLevelCount() - 1;

                                    building.SetUpgradeLevel(data.GetUpgradeLevelCount() - 1);
                                });

                                Parallel.ForEach(this.Device.Player.GameObjectManager.GetGameObjects(11), t =>
                                {
                                    var trap = (Builder_Trap) t;
                                    var data = (Traps) trap.Data;

                                    trap.SetUpgradeLevel(data.GetUpgradeLevelCount() - 1);
                                });

                                SendChatMessage("Command Processor: Success, Enjoy!");
                                new Own_Home_Data(this.Device).Send();
                            }
                            else
                                SendChatMessage("Command Processor: Please visit builder village first before running this mode!");
                            break;

                        default:
                            this.Help = new StringBuilder();
                            this.Help.AppendLine("Available village types:");
                            this.Help.AppendLine("\t 0 = Normal Village");
                            this.Help.AppendLine("\t 1 = Builder Village (Make sure you have unlock builder base first!)");
                            this.Help.AppendLine("\t 2 = All Village (Make sure you have unlock builder base first!)");
                            this.Help.AppendLine();
                            this.Help.AppendLine("Command:");
                            this.Help.AppendLine("\t/max_village {village-id}");
                            SendChatMessage(Help.ToString());
                            return;
                    }
                }
                else
                {
                    this.Help = new StringBuilder();
                    this.Help.AppendLine("Available village types:");
                    this.Help.AppendLine("\t 0 = Normal Village");
                    this.Help.AppendLine("\t 1 = Builder Village (Make sure you have unlock builder base first!)");
                    this.Help.AppendLine("\t 2 = All Village (Make sure you have unlock builder base first!)");
                    this.Help.AppendLine();
                    this.Help.AppendLine("Command:");
                    this.Help.AppendLine("\t/max_village {village-id}");
                    SendChatMessage(Help.ToString());
                }
            }
            else
            {
                this.Help = new StringBuilder();
                this.Help.AppendLine("Available village types:");
                this.Help.AppendLine("\t 0 = Normal Village");
                this.Help.AppendLine("\t 1 = Builder Village (Make sure you have unlock builder base first!)");
                this.Help.AppendLine("\t 2 = All Village (Make sure you have unlock builder base first!)");
                this.Help.AppendLine();
                this.Help.AppendLine("Command:");
                this.Help.AppendLine("\t/max_village {village-id}");
                SendChatMessage(Help.ToString());
            }
        }
    }
}