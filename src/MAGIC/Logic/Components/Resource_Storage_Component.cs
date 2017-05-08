using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Files;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Logic.Structure;

namespace BL.Servers.CoC.Logic.Components
{
    internal class Resource_Storage_Component : Component
    {
        public Resource_Storage_Component(GameObject go) : base(go)
        {
            this.CurrentResources = new List<int>();
            this.MaxResources = new List<int>();
            this.StolenResources = new List<int>();

            var table = CSV.Tables.Get(Gamefile.Resources);
            var resourceCount = table.Datas.Count;
            for (var i = 0; i < resourceCount; i++)
            {
                this.CurrentResources.Add(0);
                this.MaxResources.Add(0);
                this.StolenResources.Add(0);
            }
        }

        internal override int Type => 6;

        internal readonly List<int> CurrentResources;
        internal readonly List<int> StolenResources;
        internal List<int> MaxResources;

        public int GetCount(int resourceIndex) => this.CurrentResources[resourceIndex];

        public int GetMax(int resourceIndex) => this.MaxResources[resourceIndex];

        public void SetMaxArray(List<int> resourceCaps)
        {
            this.MaxResources = resourceCaps;
            GetParent.Avatar.GetComponentManager.RefreshResourcesCaps();
        }
    }
}
