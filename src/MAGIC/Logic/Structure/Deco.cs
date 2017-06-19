using System;
using System.Collections.Generic;
using System.Linq;
namespace CRepublic.Magic.Logic.Structure
{
    using CRepublic.Magic.Files.CSV_Helpers;
    using CRepublic.Magic.Files.CSV_Logic;

    internal class Deco : GameObject
    {
        public Deco(Data data, Level l) : base(data, l)
        {
        }

        internal override int ClassId => 6;

        public Decos GetDecoData() => (Decos)GetData();
    }
}