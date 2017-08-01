namespace Magic.ClashOfClans.Logic
{
    internal class ComponentFilter : GameObjectFilter
    {
        public int Type;

        public ComponentFilter(int type)
        {
            Type = type;
        }

        public override bool IsComponentFilter() => true;

        public bool TestComponent(Component c)
        {
            var go = c.Parent;
            return TestGameObject(go);
        }

        public new bool TestGameObject(GameObject go)
        {
            var result = false;
            var c = go.GetComponent(Type, true);
            if (c != null)
                result = base.TestGameObject(go);
            return result;
        }
    }
}