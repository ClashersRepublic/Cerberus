using System;
using System.Collections.Generic;
using System.Linq;
namespace BL.Servers.CoC.Logic.Structure
{
    using BL.Servers.CoC.Files.CSV_Helpers;
    using BL.Servers.CoC.Files.CSV_Logic;
    using Newtonsoft.Json.Linq;

    internal class Builder_Deco : GameObject
    {
        internal Level Level;

        public Builder_Deco(Data data, Level l) : base(data, l)
        {
            this.Level = l;
        }

        internal override int ClassId => 13;

        public Decos GetDecoData() => (Decos)GetData();

    }
}