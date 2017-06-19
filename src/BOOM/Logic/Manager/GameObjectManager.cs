using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Boom.Files;
using CRepublic.Boom.Files.CSV_Helpers;
using CRepublic.Boom.Files.CSV_Logic;
using CRepublic.Boom.Logic.Enums;
using CRepublic.Boom.Logic.Structure;
using Newtonsoft.Json.Linq;

namespace CRepublic.Boom.Logic.Manager
{
    internal class GameObjectManager
    {
        internal GameObjectManager(Level l)
        {
            this.Level = l;
            this.GameObjects = new List<List<GameObject>>();
            this.GameObjectRemoveList = new List<GameObject>();
            this.GameObjectsIndex = new List<int>();
            for (int i = 0; i < 7; i++)
            {
                this.GameObjects.Add(new List<GameObject>());
                this.GameObjectsIndex.Add(0);
            }
            this.ComponentManager = new ComponentManager(this.Level);
        }

        internal readonly ComponentManager ComponentManager;
        internal readonly List<GameObject> GameObjectRemoveList;
        internal readonly List<List<GameObject>> GameObjects;
        internal readonly List<int> GameObjectsIndex;
        internal readonly Level Level;

        internal ComponentManager GetComponentManager() => this.ComponentManager;
        internal void AddGameObject(GameObject go)
        {
            go.GlobalId = GenerateGameObjectGlobalId(go);
            if (go.ClassId == 0)
            {
                var b = (Building)go;
                //var bd = b.GetBuildingData();
               // if (bd.IsWorkerBuilding())
                 //   m_vLevel.WorkerManager.IncreaseWorkerCount();
            }
            this.GameObjects[go.ClassId].Add(go);
        }

        internal int GenerateGameObjectGlobalId(GameObject go)
        {
            var index = this.GameObjectsIndex[go.ClassId];
            this.GameObjectsIndex[go.ClassId]++;
            return GlobalID.CreateGlobalID(go.ClassId + 500, index);
        }

        internal void RemoveGameObjectTotally(GameObject go)
        {
            this.GameObjects[go.ClassId].Remove(go);
            if (go.ClassId == 0)
            {
                var b = (Building)go;
                var bd = b.GetBuildingData();
                //if (bd.IsWorkerBuilding())
                   // m_vLevel.WorkerManager.DecreaseWorkerCount();
            }
            RemoveGameObjectReferences(go);
        }

        public List<List<GameObject>> GetAllGameObjects() => this.GameObjects;
        public GameObject GetGameObjectByID(int id)
        {
            var classId = GlobalID.GetType(id) - 500;
            if (this.GameObjects.Capacity < classId)
                return null;
            return this.GameObjects[classId].Find(g => g.GlobalId == id);
        }
        public void RemoveGameObject(GameObject go)
        {
            this.GameObjects[go.ClassId].Remove(go);
            if (go.ClassId == 0)
            {
                var b = (Building)go;
                //var bd = b.GetBuildingData();
               // if (bd.IsWorkerBuilding())
                {
                 //   m_vLevel.WorkerManager.DecreaseWorkerCount();
                }
            }
            RemoveGameObjectReferences(go);
        }

        public void RemoveGameObjectReferences(GameObject go)
        {
            this.ComponentManager.RemoveGameObjectReferences(go);
        }
        public List<GameObject> GetGameObjects(int id) => this.GameObjects[id];

        public void Load(JObject jsonObject)
        {
            var jsonBuildings = (JArray) jsonObject["buildings"];
            foreach (JObject jsonBuilding in jsonBuildings)
            {
                var bd = CSV.Tables.Get(Gamefile.Buildings).GetDataWithID(jsonBuilding["data"].ToObject<int>()) as Buildings;
                var b = new Building(bd, this.Level);
                AddGameObject(b);
                b.Load(jsonBuilding);
            }
            var jsonTraps = (JArray)jsonObject["traps"];
            foreach (JObject jsonTrap in jsonTraps)
            {
                var td = CSV.Tables.Get(Gamefile.Traps).GetDataWithID(jsonTrap["data"].ToObject<int>()) as Traps;
                var t = new Trap(td, this.Level);
                AddGameObject(t);
                t.Load(jsonTrap);
            }

            var jsonDecos = (JArray)jsonObject["decos"];
            foreach (JObject jsonDeco in jsonDecos)
            {
                var dd = CSV.Tables.GetWithGlobalID(jsonDeco["data"].ToObject<int>()) as Decos;
                var d = new Deco(dd, this.Level);
                AddGameObject(d);
                d.Load(jsonDeco);
            }

            var jsonObstacles = (JArray)jsonObject["obstacles"];
            foreach (JObject jsonObstacle in jsonObstacles)
            {
                var dd = CSV.Tables.Get(Gamefile.Obstacles).GetDataWithID(jsonObstacle["data"].ToObject<int>()) as Obstacles;
                var d = new Obstacle(dd, this.Level);
                AddGameObject(d);
                d.Load(jsonObstacle);
            }

        }

        public JObject Save()
        {
            JArray JBuildings = new JArray();
            int c = 0;
            foreach (GameObject go in new List<GameObject>(this.GameObjects[0]))
            {
                Building b = (Building)go;
                JObject j = new JObject { { "data", b.GetBuildingData().GetGlobalID() }, { "id", 500000000 + c } };
                b.Save(j);
                JBuildings.Add(j);
                c++;
            }

            JArray JTraps = new JArray();
            int u = 0;
            foreach (GameObject go in new List<GameObject>(this.GameObjects[4]))
            {
                Trap t = (Trap)go;
                JObject j = new JObject {{"data", t.GetTrapData().GetGlobalID()}, {"id", 504000000 + u}};
                t.Save(j);
                JTraps.Add(j);
                u++;
            }

             JArray JObstacles = new JArray();
            int o = 0;
            foreach (GameObject go in new List<GameObject>(this.GameObjects[3]))
            {
                Obstacle d = (Obstacle)go;
                JObject j = new JObject {{"data", d.GetObstacleData().GetGlobalID()}, {"id", 503000000 + o}};
                d.Save(j);
                JObstacles.Add(j);
                o++;
            }

            JArray JDecos = new JArray();
            int e = 0;
            foreach (GameObject go in new List<GameObject>(this.GameObjects[6]))
            {
                Deco d = (Deco) go;
                JObject j = new JObject {{"data", d.GetDecoData().GetGlobalID()}, {"id", 506000000 + e}};
                d.Save(j);
                JDecos.Add(j);
                e++;
            }

            Avatar pl = this.Level.Avatar;
            var jsonData = new JObject
            {
                //{"exp_ver", 1},
                {"buildings", JBuildings},
                {"obstacles", JObstacles},
                {"traps", JTraps},
                {"decos", JDecos}
            };
            return jsonData;
        }
        public void Tick()
        {
            this.ComponentManager.Tick();
            foreach (var l in this.GameObjects)
            {
                foreach (var go in l)
                    go.Tick();
            }
            foreach (var g in new List<GameObject>(this.GameObjectRemoveList))
            {
                RemoveGameObjectTotally(g);
                this.GameObjectRemoveList.Remove(g);
            }
        }
    }
}
