using System;
using System.Collections.Generic;
using System.Linq;
namespace BL.Servers.CoC.Logic.Structure
{
    using BL.Servers.CoC.Files.CSV_Helpers;
    using BL.Servers.CoC.Files.CSV_Logic;
    using Newtonsoft.Json.Linq;

    internal class Deco : GameObject
    {
        internal Level Level;

        public Deco(Data data, Level l) : base(data, l)
        {
            this.Level = l;
        }

        internal override int ClassId => 6;

        public Decos GetDecoData() => (Decos)GetData();

        public new void Load(JObject jsonObject)
        {
            base.Load(jsonObject);
        }

        public new JObject Save(JObject jsonObject)
        {
            base.Save(jsonObject);
            return jsonObject;
        }
    }
}