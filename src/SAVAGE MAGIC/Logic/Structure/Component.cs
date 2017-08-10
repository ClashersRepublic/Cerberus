using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CRepublic.Magic.Logic.Structure
{
    internal class Component
    {
        public Component()
        {
            // Space
        }

        public Component(GameObject parent)
        {
            Parent = parent;
        }

        public virtual int Type => -1;

        public GameObject Parent { get; }

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
