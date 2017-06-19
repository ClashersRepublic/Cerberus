using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Magic.Files.CSV_Helpers;
using CRepublic.Magic.Files.CSV_Logic;

namespace CRepublic.Magic.Logic.Structure
{
    internal class Village_Object : ConstructionItem
    {
        public Village_Object(Data data, Level level) : base(data, level)
        {
        }

        internal override int ClassId => 8;

        public Village_Objects GetVillageObjectsData => (Village_Objects)GetData();
    }
}
