using System;
using System.Collections.Generic;
using System.Linq;
namespace Republic.Magic.Logic.Structure
{
    using Republic.Magic.Files.CSV_Helpers;
    using Republic.Magic.Files.CSV_Logic;

    internal class Deco : GameObject
    {
        public Deco(Data data, Level l) : base(data, l)
        {
        }

        internal override int ClassId => 6;

        public Decos GetDecoData() => (Decos)GetData();
    }
}