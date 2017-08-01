using Newtonsoft.Json.Linq;

namespace Magic.ClashOfClans.Logic
{
    internal class Component
    {
        public Component()
        {
            // Space
        }

        public Component(GameObject parent)
        {
            _parent = parent;
        }

        private readonly GameObject _parent;
        public virtual int Type => -1;

        public GameObject Parent => _parent;
        public bool IsEnabled { get; set; }

        public virtual void Load(JObject jsonObject)
        {
            // Space
        }

        public virtual JObject Save(JObject jsonObject)
        {
            return jsonObject;
        }

        public virtual void Tick()
        {
            // Space
        }
    }
}