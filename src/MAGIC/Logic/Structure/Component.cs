namespace BL.Servers.CoC.Logic.Structure
{
    using Newtonsoft.Json.Linq;
    internal class Component
    {

        internal virtual int Type => -1;

        internal readonly GameObject ParentGameObject;
        internal bool IsEnable;

        internal Component()
        {
        }

        internal Component(GameObject go)
        {
            this.IsEnable = true;
            this.ParentGameObject = go;
        }

        internal GameObject GetParent => this.ParentGameObject;

        internal virtual void Load(JObject jsonObject)
        {
        }

        internal virtual JObject Save(JObject jsonObject) => jsonObject;

        internal void SetEnabled(bool status) => this.IsEnable = status;

        internal virtual void Tick()
        {
        }
    }
}