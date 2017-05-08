using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Files.CSV_Helpers;
using BL.Servers.CoC.Files.CSV_Reader;

namespace BL.Servers.CoC.Files.CSV_Logic
{
    internal class Combat_Item : Data
    {
        public Combat_Item(Row row, DataTable dt) : base(row, dt)
        {
        }

        public virtual int GetCombatItemType() => -1;

        public virtual int GetHousingSpace() => -1;

        public virtual int GetRequiredLaboratoryLevel(int level) => -1;

        public virtual int GetRequiredProductionHouseLevel() => -1;

        public virtual int GetTrainingCost(int level) => -1;

        public virtual Resource GetTrainingResource() => null;

        public virtual int GetTrainingTime() => -1;

        public virtual int GetUpgradeCost(int level) => -1;

        public virtual int GetUpgradeLevelCount() => -1;

        public virtual Resource GetUpgradeResource(int level) => null;

        public virtual int GetUpgradeTime(int level) => -1;
    }
}
