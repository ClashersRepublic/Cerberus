using System.Collections.Generic;
using System.Windows;
using Magic.ClashOfClans.Core;
using Magic.Files.Logic;

namespace Magic.ClashOfClans.Logic.Manager
{
    internal class ComponentManager
    {
        public ComponentManager(Level l)
        {
            m_vComponents = new List<List<Component>>();
            for (var i = 0; i <= 10; i++)
                m_vComponents.Add(new List<Component>());
            m_vLevel = l;
        }

        readonly List<List<Component>> m_vComponents;

        readonly Level m_vLevel;

        public void AddComponent(Component c) => m_vComponents[c.Type].Add(c);

        public Component GetClosestComponent(int x, int y, ComponentFilter cf)
        {
            Component result = null;
            var componentType = cf.Type;
            var components = m_vComponents[componentType];
            var v = new Vector(x, y);
            double maxLengthSquared = 0;

            if (components.Count > 0)
                foreach (var c in components)
                    if (cf.TestComponent(c))
                    {
                        var go = c.Parent;
                        var lengthSquared = (v - go.GetPosition()).LengthSquared;
                        if (lengthSquared < maxLengthSquared || result == null)
                        {
                            maxLengthSquared = lengthSquared;
                            result = c;
                        }
                    }
            return result;
        }

        public List<Component> GetComponents(int type) => m_vComponents[type];

        public int GetMaxBarrackLevel()
        {
            var result = 0;
            var components = m_vComponents[3];
            if (components.Count > 0)
                foreach (UnitProductionComponent c in components)
                    if (!c.IsSpellForge())
                    {
                        var level = ((Building) c.Parent).GetUpgradeLevel();
                        if (level > result)
                            result = level;
                    }
            return result;
        }

        public int GetMaxSpellForgeLevel()
        {
            var result = 0;
            var components = m_vComponents[3];
            if (components.Count > 0)
                foreach (UnitProductionComponent c in components)
                    if (c.IsSpellForge())
                    {
                        var b = (Building) c.Parent;
                        if (!b.IsConstructing() || b.IsUpgrading())
                        {
                            var level = b.GetUpgradeLevel();
                            if (level > result)
                                result = level;
                        }
                    }
            return result;
        }

        public int GetTotalMaxHousing(bool IsSpellForge = false)
        {
            var result = 0;
            var components = m_vComponents[0];
            if (components.Count >= 1)
                foreach (var c in components)
                    if (((UnitStorageComponent) c).IsSpellForge == IsSpellForge)
                        result += ((UnitStorageComponent) c).GetMaxCapacity();
            return result;
        }

        public int GetTotalUsedHousing(bool IsSpellForge = false)
        {
            var result = 0;
            var components = m_vComponents[0];
            if (components.Count >= 1)
                foreach (var c in components)
                    if (((UnitStorageComponent) c).IsSpellForge == IsSpellForge)
                        result += ((UnitStorageComponent) c).GetUsedCapacity();
            return result;
        }

        public void RefreshResourcesCaps()
        {
            var table = CsvManager.DataTables.GetTable(2);
            var resourceCount = table.GetItemCount();
            var resourceStorageComponentCount = GetComponents(6).Count;
            for (var i = 0; i < resourceCount; i++)
            {
                var resourceCap = 0;
                for (var j = 0; j < resourceStorageComponentCount; j++)
                {
                    var res = (ResourceStorageComponent) GetComponents(6)[j];
                    if (res.IsEnabled)
                        resourceCap += res.GetMax(i);
                    var resource = (ResourceData) table.GetItemAt(i);
                    if (!resource.PremiumCurrency)
                        m_vLevel.Avatar.SetResourceCap(resource, resourceCap);
                }
            }
        }

        public void RemoveGameObjectReferences(GameObject go)
        {
            foreach (var components in m_vComponents)
            {
                var markedForRemoval = new List<Component>();
                foreach (var component in components)
                    if (component.Parent== go)
                        markedForRemoval.Add(component);
                foreach (var component in markedForRemoval)
                    components.Remove(component);
            }
        }

        public void Tick()
        {
        }
    }
}
