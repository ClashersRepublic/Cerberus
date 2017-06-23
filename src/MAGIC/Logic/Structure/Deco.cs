using CRepublic.Magic.Files.CSV_Helpers;
using CRepublic.Magic.Files.CSV_Logic;

namespace CRepublic.Magic.Logic.Structure
{

    internal class Deco : GameObject
    {
        public Deco(Data data, Level l) : base(data, l)
        {
        }

        internal override int ClassId => 6;

        public Decos GetDecoData() => (Decos)GetData();
    }
}