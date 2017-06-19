using System;
using System.Collections.Generic;
using System.Linq;
namespace Republic.Magic.Logic.Structure
{
    using Republic.Magic.Files.CSV_Helpers;
    using Republic.Magic.Files.CSV_Logic;
    using Newtonsoft.Json.Linq;

    internal class Builder_Deco : GameObject
    {
        public Builder_Deco(Data data, Level l) : base(data, l)
        {
        }

        internal override int ClassId => 13;

        public Decos GetDecoData => (Decos)GetData();

    }
}