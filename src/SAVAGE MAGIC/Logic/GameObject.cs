using Magic.Files.Logic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Magic.ClashOfClans.Logic
{
    internal class GameObject
    {
        public GameObject(Data data, Level level)
        {
            _level = level;
            _data = data;
            _components = new List<Component>();
            for (var i = 0; i < 11; i++)
                _components.Add(new Component());
        }

        private readonly List<Component> _components;
        private readonly Data _data;
        private readonly Level _level;

        public virtual int ClassId => -1;

        public int GlobalId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int L1X { get; set; }
        public int L1Y { get; set; }
        public int L2X { get; set; }
        public int L2Y { get; set; }
        public int L3X { get; set; }
        public int L3Y { get; set; }

        public Data Data => _data;
        public Level Level => _level;

        [Obsolete]
        public int TownHallLevel => Level.Avatar.GetTownHallLevel();
        [Obsolete]
        public virtual bool IsHero => false;
        [Obsolete]
        public int LayoutId => Level.Avatar.GetActiveLayout();

        //TODO: Remove dependency on System.Windows.
        public Vector GetPosition() => new Vector(X, Y);

        public void AddComponent(Component component)
        {
            // ??
            if (_components[component.Type].Type != -1)
            {
            }
            else
            {
                _level.GetComponentManager().AddComponent(component);
                _components[component.Type] = component;
            }
        }

        public Component GetComponent(int index, bool test)
        {
            Component result = null;
            if (!test || _components[index].IsEnabled)
                result = _components[index];
            return result;
        }

        public void Load(JObject jsonObject)
        {
            X = jsonObject["x"].ToObject<int>();
            Y = jsonObject["y"].ToObject<int>();

            if (TownHallLevel >= 4)
            {
                L1X = jsonObject["l1x"]?.ToObject<int>() ?? 0;
                L1Y = jsonObject["l1y"]?.ToObject<int>() ?? 0;

                L2X = jsonObject["l2x"]?.ToObject<int>() ?? 0;
                L2Y = jsonObject["l2y"]?.ToObject<int>() ?? 0;

                if (TownHallLevel >= 6)
                {
                    L3X = jsonObject["l3x"]?.ToObject<int>() ?? 0;
                    L3Y = jsonObject["l3y"]?.ToObject<int>() ?? 0;
                }
            }

            foreach (Component c in _components)
                c.Load(jsonObject);
        }

        public JObject Save(JObject jsonObject)
        {
            jsonObject.Add("x", X);
            jsonObject.Add("y", Y);

            if (TownHallLevel >= 4)
            {
                if (LayoutId == 2)
                {
                    jsonObject.Add("lmx", L2X);
                    jsonObject.Add("lmy", L2Y);
                }
                else if (LayoutId == 3)
                {
                    jsonObject.Add("lmx", L3X);
                    jsonObject.Add("lmy", L3Y);
                }

                jsonObject.Add("l1x", L1X);
                jsonObject.Add("l1y", L1Y);

                jsonObject.Add("l2x", L2X);
                jsonObject.Add("l2y", L2Y);

                if (TownHallLevel >= 6)
                {
                    jsonObject.Add("l3x", L3X);
                    jsonObject.Add("l3y", L3Y);
                }
            }
            foreach (Component c in _components)
                c.Save(jsonObject);
            return jsonObject;
        }

        public void SetPosition(int newX, int newY, int layoutIndex)
        {
            if (layoutIndex == LayoutId)
            {
                X = newX;
                Y = newY;
            }

            if (layoutIndex == 1)
            {
                L1X = newX;
                L1Y = newY;
            }
            else if (layoutIndex == 2)
            {
                L2X = newX;
                L2Y = newY;
            }
            else if (layoutIndex == 3)
            {
                L3X = newX;
                L3Y = newY;
            }
        }

        public virtual void Tick()
        {
            foreach (var comp in _components)
            {
                if (comp.IsEnabled)
                    comp.Tick();
            }
        }
    }
}