using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Republic.Magic.Core;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Files;
using Republic.Magic.Files.CSV_Logic;
using Republic.Magic.Logic;
using Republic.Magic.Logic.Enums;
using Republic.Magic.Logic.Structure;

namespace Republic.Magic.Packets.Commands.Client
{
    internal class Upgrade_Unit : Command
    {
        internal int BuidlingID;
        internal int GlobalId;
        internal int Tick;
        internal bool IsSpell;
        internal Characters Troop;
        internal Spells Spell;
        public Upgrade_Unit(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.BuidlingID = this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            this.GlobalId = this.Reader.ReadInt32();
            if (GlobalId >= 26000000)
            {
                this.IsSpell = true;
                this.Spell = CSV.Tables.Get(Gamefile.Spells).GetDataWithID(GlobalId) as Spells;
            }
            else
            {
                this.Troop = CSV.Tables.Get(Gamefile.Characters).GetDataWithID(GlobalId) as Characters;
            }
            this.Tick = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            var ca = this.Device.Player.Avatar;
            var go = this.Device.Player.Avatar.Variables.IsBuilderVillage ? this.Device.Player.GameObjectManager.GetBuilderVillageGameObjectByID(this.BuidlingID) : this.Device.Player.GameObjectManager.GetGameObjectByID(this.BuidlingID);

            if (go != null)
            {
                if (!ca.Variables.IsBuilderVillage)
                {
                    var building = (Building) go;
                    var upgradeComponent = building.GetUnitUpgradeComponent();


                    var unitLevel = ca.GetUnitUpgradeLevel(this.IsSpell ? (Combat_Item) this.Spell : (Combat_Item) this.Troop);
                    
                    if (upgradeComponent.CanStartUpgrading(this.IsSpell ? (Combat_Item) this.Spell  : (Combat_Item) this.Troop))
                    {
                        var cost = this.IsSpell ? this.Spell.GetUpgradeCost(unitLevel) : this.Troop.GetUpgradeCost(unitLevel);
                        var upgradeResource = this.IsSpell
                            ? this.Spell.GetUpgradeResource()
                            : this.Troop.GetUpgradeResource();
                        if (ca.HasEnoughResources(upgradeResource.GetGlobalID(), cost))
                        {
#if DEBUG
                            Loggers.Log(
                                this.IsSpell
                                    ? $"Spell : Upgrading {this.Spell.Row.Name} with ID {GlobalId}"
                                    : $"Unit : Upgrading {this.Troop.Row.Name} with ID {GlobalId}", true);
#endif
                            ca.Resources.Minus(upgradeResource.GetGlobalID(), cost);
                            upgradeComponent.StartUpgrading(this.IsSpell
                                ? (Combat_Item) this.Spell
                                : (Combat_Item) this.Troop);
                        }
                    }
                }
                else
                {
                    var building = (Builder_Building) go;
                    var upgradeComponent = building.GetUnitUpgradeComponent();


                    var unitLevel = ca.GetUnitUpgradeLevel(this.IsSpell
                        ? (Combat_Item) this.Spell
                        : (Combat_Item) this.Troop);
                    
                    if (upgradeComponent.CanStartUpgrading(this.IsSpell ? (Combat_Item)this.Spell : (Combat_Item)this.Troop))
                    {
                        var cost = this.IsSpell ? this.Spell.GetUpgradeCost(unitLevel) : this.Troop.GetUpgradeCost(unitLevel);
                        var upgradeResource = this.IsSpell
                            ? this.Spell.GetUpgradeResource()
                            : this.Troop.GetUpgradeResource();
                        if (ca.HasEnoughResources(upgradeResource.GetGlobalID(), cost))
                        {
#if DEBUG
                            Loggers.Log(
                                this.IsSpell
                                    ? $"Spell : Upgrading {this.Spell.Row.Name} with ID {GlobalId}"
                                    : $"Unit : Upgrading {this.Troop.Row.Name} with ID {GlobalId}", true);
#endif
                            ca.Resources.Minus(upgradeResource.GetGlobalID(), cost);
                            upgradeComponent.StartUpgrading(this.IsSpell
                                ? (Combat_Item) this.Spell
                                : (Combat_Item) this.Troop);
                        }
                    }
                }
            }
        }
    }
}
