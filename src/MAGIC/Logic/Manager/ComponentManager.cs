using System;
using System.Collections.Generic;
using System.Windows;
using BL.Servers.CoC.Files;
using BL.Servers.CoC.Files.CSV_Logic;
using BL.Servers.CoC.Logic.Components;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Logic.Structure;

namespace BL.Servers.CoC.Logic.Manager
{
    internal class ComponentManager
    {
        public ComponentManager(Level l)
        {
            this.Components = new List<List<Component>>();
            for (var i = 0; i <= 10; i++)
                this.Components.Add(new List<Component>());
            this.Level = l;
        }

        readonly List<List<Component>> Components;

        readonly Level Level;

        public void AddComponent(Component c) => this.Components[c.Type].Add(c);

        public Component GetClosestComponent(int x, int y, ComponentFilter cf)
        {
            Component result = null;
            var componentType = cf.Type;
            var components = this.Components[componentType];
            var v = new Vector(x, y);
            double maxLengthSquared = 0;

            if (components.Count > 0)
            {
                foreach (var c in components)
                {
                    if (cf.TestComponent(c))
                    {
                        var go = c.GetParent;
                        var lengthSquared = (v - go.GetPosition()).LengthSquared;
                        if (lengthSquared < maxLengthSquared || result == null)
                        {
                            maxLengthSquared = lengthSquared;
                            result = c;
                        }
                    }
                }
            }
            return result;
        }

        public List<Component> GetComponents(int type) => this.Components[type];

        public int GetMaxBarrackLevel()
        {
            var result = 0;
            var components = this.Components[3];
            if (components.Count > 0)
                //foreach (UnitProductionComponent c in components)
                  //  if (!c.IsSpellForge())
                    {
                    //    var level = ((Building) c.GetParent()).GetUpgradeLevel();
                      //  if (level > result)
                        //    result = level;
                    }
            return result;
        }

        public int GetMaxSpellForgeLevel()
        {
            var result = 0;
            var components = this.Components[3];
            if (components.Count > 0)
                //foreach (UnitProductionComponent c in components)
                  //  if (c.IsSpellForge())
                    //{
                      //  var b = (Building) c.GetParent();
                       // if (!b.IsConstructing || b.IsUpgrading())
                        {
                        //    var level = b.GetUpgradeLevel();
                         //   if (level > result)
                           //     result = level;
                        //}
                    }
            return result;
        }

        public int GetTotalMaxHousing(bool IsSpellForge = false)
        {
            var result = 0;
            var components = this.Components[0];
            //if (components.Count >= 1)
            //    foreach (var c in components)
                  //  if (((UnitStorageComponent) c).IsSpellForge == IsSpellForge)
                    //    result += ((UnitStorageComponent) c).GetMaxCapacity();
            return result;
        }

        public int GetTotalUsedHousing(bool IsSpellForge = false)
        {
            var result = 0;
            var components = this.Components[0];
            //if (components.Count >= 1)
              //  foreach (var c in components)
                   // if (((UnitStorageComponent) c).IsSpellForge == IsSpellForge)
                    //    result += ((UnitStorageComponent) c).GetUsedCapacity();
            return result;
        }

        public void RefreshResourcesCaps()
        {
            var table = CSV.Tables.Get(Gamefile.Resources);
            var resourceCount = table.Datas.Count;
            var resourceStorageComponentCount = GetComponents(6).Count;
            for (var i = 0; i < resourceCount; i++)
            {
                var resourceCap = 0;
                for (var j = 0; j < resourceStorageComponentCount; j++)
                {
                    var res = (Resource_Storage_Component) GetComponents(6)[j];
                    if (res.IsEnable)
                        resourceCap += res.GetMax(i);
                    var resource = (Files.CSV_Logic.Resource)table.Datas[i];
                    if (!resource.PremiumCurrency)
                    {
                        this.Level.Avatar.Resources_Cap.Set(resource.GetGlobalID(), resourceCap);
                    }
                }
            }
        }

        public void RemoveGameObjectReferences(GameObject go)
        {
            foreach (var components in this.Components)
            {
                var markedForRemoval = new List<Component>();
                foreach (var component in components)
                    if (component.GetParent == go)
                        markedForRemoval.Add(component);
                foreach (var component in markedForRemoval)
                    components.Remove(component);
            }
        }

        internal void Tick()
        {
        }
    }
}
