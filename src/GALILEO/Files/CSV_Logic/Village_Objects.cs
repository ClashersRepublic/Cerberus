using BL.Servers.CoC.Files.CSV_Helpers;
using BL.Servers.CoC.Files.CSV_Reader;

namespace BL.Servers.CoC.Files.CSV_Logic
{
    internal class Village_Objects : Construction_Item
    {
        public Village_Objects(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(Row);
        }

        public string Name { get; set; }
        public string TID { get; set; }
        public string InfoTID { get; set; }
        public string[] SWF { get; set; }
        public string[] ExportName { get; set; }
        public int[] TileX100 { get; set; }
        public int[] TileY100 { get; set; }
        public int RequiredTH { get; set; }
        public int[] BuildTimeD { get; set; }
        public int[] BuildTimeH { get; set; }
        public int[] BuildTimeM { get; set; }
        public int[] BuildTimeS { get; set; }
        public bool RequiresBuilder { get; set; }
        public string BuildResource { get; set; }
        public int BuildCost { get; set; }
        public int[] TownHallLevel { get; set; }
        public string PickUpEffect { get; set; }
        public string Animations { get; set; }
        public int AnimX { get; set; }
        public int AnimY { get; set; }
        public int AnimID { get; set; }
        public int AnimDir { get; set; }
        public int AnimVisibilityOdds { get; set; }
        public bool HasInfoScreen { get; set; }
        public int VillageType { get; set; }
        public int UnitHousing { get; set; }
    }
}
