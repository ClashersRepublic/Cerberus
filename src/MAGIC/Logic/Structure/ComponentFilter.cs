using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CoC.Logic.Structure
{
    internal class ComponentFilter : GameObjectFilter
    {
        internal int Type;

        public ComponentFilter(int type)
        {
            this.Type = type;
        }

        public override bool IsComponentFilter() => true;

        public bool TestComponent(Component c)
        {
            GameObject go = c.GetParent;
            return this.TestGameObject(go);
        }

        public new bool TestGameObject(GameObject go)
        {
            bool result = false;
            Component c = go.GetComponent(this.Type, true);
            if (c != null)
            {
                result = base.TestGameObject(go);
            }
            return result;
        }
    }
}