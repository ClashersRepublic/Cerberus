using Newtonsoft.Json.Linq;
using Magic.Files.Logic;

namespace Magic.ClashOfClans.Logic
{
    internal class Deco : GameObject
    {
        Level m_vLevel;

        public Deco(Data data, Level l) : base(data, l)
        {
            m_vLevel = l;
        }

        public override int ClassId => 6;

        public DecoData GetDecoData() => (DecoData)Data;

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