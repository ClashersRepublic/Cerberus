using System;
using System.Collections.Generic;
using System.Linq;
namespace BL.Servers.CoC.Logic.Structure
{
    using BL.Servers.CoC.Files.CSV_Helpers;
    using BL.Servers.CoC.Files.CSV_Logic;

    internal class Deco : GameObject
    {
        public Deco(Data data, Level l) : base(data, l)
        {
        }

        internal override int ClassId => 6;

        public Decos GetDecoData() => (Decos)GetData();
    }
}