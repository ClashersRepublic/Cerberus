using CRepublic.Magic.Files.CSV_Helpers;
using CRepublic.Magic.Files.CSV_Reader;

namespace CRepublic.Magic.Files.CSV_Logic
{
    internal class Variables : Data
    {
        public Variables(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(Row);
        }

        public string Name { get; set; }
    }
}
