using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CRepublic.Boom.Files;
using CRepublic.Boom.Logic.Components;
using CRepublic.Boom.Logic.Enums;
using CRepublic.Boom.Logic.Structure;
using Resource = CRepublic.Boom.Files.CSV_Logic.Resource;

namespace CRepublic.Boom.Logic.Manager
{
    internal class ComponentManager
    {
        internal ComponentManager(Level l)
        {
            this.Components = new List<List<Component>>();
            for (var i = 0; i <= 10; i++)
                this.Components.Add(new List<Component>());
            this.Level = l;
        }

        readonly List<List<Component>> Components;

        readonly Level Level;
        internal void AddComponent(Component c) => this.Components[c.Type].Add(c);

        internal List<Component> GetComponents(int type) => this.Components[type];

        internal void RefreshResourcesCaps()
        {
            var table = CSV.Tables.Get(Gamefile.Resources);
            var resourceCount = table.Datas.Count;
            var resourceStorageComponentCount = GetComponents(6).Count;
            for (var i = 0; i < resourceCount; i++)
            {
                var resourceCap = 0;
                for (var j = 0; j < resourceStorageComponentCount; j++)
                {
                    var res = (Resource_Storage_Component)GetComponents(6)[j];
                    if (res.IsEnabled())
                        resourceCap += res.GetMax(i);
                    var resource = (Resource)table.Datas[i];
                    if (!resource.PremiumCurrency)
                        this.Level.Avatar.Resources_Cap.Set(resource.GetGlobalID(), resourceCap);
                }
            }
        }

        internal Component GetClosestComponent(int x, int y, ComponentFilter cf)
        {
            Component result = null;
            var componentType = cf.Type;
            var components = this.Components[componentType];
            var v = new Vector(x, y);
            double maxLengthSquared = 0;

            if (components.Count > 0)
                foreach (var c in components)
                    if (cf.TestComponent(c))
                    {
                        var go = c.GetParent();
                        var lengthSquared = (v - go.GetPosition()).LengthSquared;
                        if (lengthSquared < maxLengthSquared || result == null)
                        {
                            maxLengthSquared = lengthSquared;
                            result = c;
                        }
                    }
            return result;
        }
        internal void RemoveGameObjectReferences(GameObject go)
        {
            foreach (var components in this.Components)
            {
                var markedForRemoval = new List<Component>();
                foreach (var component in components)
                    if (component.GetParent() == go)
                        markedForRemoval.Add(component);
                foreach (var component in markedForRemoval)
                    components.Remove(component);
            }
        }
        internal int GetTotalMaxHousing()
        {
            var result = 0;
            var components = this.Components[0];
            if (components.Count >= 1)
                foreach (var c in components)
                {
                    
                }
            return result;
        }
        internal int GetTotalUsedHousing(bool IsSpellForge = false)
        {
            var result = 0;
            var components = this.Components[0];
            if (components.Count >= 1)
                foreach (var c in components)
                {
                    
                }
            return result;
        }
        public void Tick()
        {
        }
    }
}
