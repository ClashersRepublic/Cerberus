using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BL.Servers.CoC.Files.CSV_Helpers;
using Newtonsoft.Json.Linq;

namespace BL.Servers.CoC.Logic.Structure
{
    internal class GameObject
    {
        public GameObject(Data data, Level level)
        {
            this.Level = level;
            this.Data = data;
            this.Components = new List<Component>();
            for (var i = 0; i < 11; i++)
            {
                this.Components.Add(new Component());
            }
        }

        readonly List<Component> Components;
        readonly Data Data;
        readonly Level Level;

        internal virtual int ClassId => -1;

        internal int GlobalId;

        internal int X;

        internal int Y;

        public void AddComponent(Component c)
        {
            if (this.Components[c.Type].Type != -1)
            {
            }
            else
            {
                //this.Level.GetComponentManager().AddComponent(c);
                this.Components[c.Type] = c;
            }
        }

        public Component GetComponent(int index, bool test)
        {
            Component result = null;
            if (!test || this.Components[index].IsEnable)
            {
                result = this.Components[index];
            }
            return result;
        }

        public Data GetData() => this.Data;
        public Level Avatar => this.Level;

        public Vector GetPosition() => new Vector(X, Y);

        public virtual bool IsHero() => false;

        public void Load(JObject jsonObject)
        {
            X = jsonObject["x"].ToObject<int>();
            Y = jsonObject["y"].ToObject<int>();

            /* if (TownHallLevel() >= 4)
             {
                 L1X = jsonObject["l1x"].ToObject<int>();
                 L1Y = jsonObject["l1y"].ToObject<int>();

                 L2X = jsonObject["l2x"].ToObject<int>();
                 L2Y = jsonObject["l2y"].ToObject<int>();

                 if (TownHallLevel() >= 6)
                 {
                     L3X = jsonObject["l3x"].ToObject<int>();
                     L3Y = jsonObject["l3y"].ToObject<int>();
                 }
             } */

            foreach (Component c in this.Components)
            {
                c.Load(jsonObject);
            }
        }

        public JObject Save(JObject jsonObject)
        {
            jsonObject.Add("x", X);
            jsonObject.Add("y", Y);

            /*if (TownHallLevel() >= 4)
            {
                if (LayoutID() == 2)
                {
                    jsonObject.Add("lmx", L2X);
                    jsonObject.Add("lmy", L2Y);
                }
                else if (LayoutID() == 3)
                {
                    jsonObject.Add("lmx", L3X);
                    jsonObject.Add("lmy", L3Y);
                }

                jsonObject.Add("l1x", L1X);
                jsonObject.Add("l1y", L1Y);

                jsonObject.Add("l2x", L2X);
                jsonObject.Add("l2y", L2Y);

                if (TownHallLevel() >= 6)
                {
                    jsonObject.Add("l3x", L3X);
                    jsonObject.Add("l3y", L3Y);
                }  
            }*/

            foreach (Component c in this.Components)
            {
                c.Save(jsonObject);
            }

            return jsonObject;
        }

        public void SetPositionXY(int newX, int newY, int Layout)
        {
            /*if (Layout == LayoutID())
            {*/
            X = newX;
            Y = newY;
            //}
            /*if (Layout == Convert.ToInt32(Layouts.Layout.WarLayout1))
            {
                L1X = newX;
                L1Y = newY;
            }
            else if (Layout == Convert.ToInt32(Layouts.Layout.Layout2))
            {
                L2X = newX;
                L2Y = newY;
            }
            else if (Layout == Convert.ToInt32(Layouts.Layout.Layout3))
            {
                L3X = newX;
                L3Y = newY;
            } */
        }

        public virtual void Tick()
        {
            foreach (Component comp in this.Components)
            {
                if (comp.IsEnable)
                    comp.Tick();
            }
        }
    }
}