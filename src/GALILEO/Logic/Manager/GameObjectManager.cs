using System.Collections.Generic;
using BL.Servers.CoC.Files;
using BL.Servers.CoC.Files.CSV_Helpers;
using BL.Servers.CoC.Files.CSV_Logic;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Logic.Manager;
using BL.Servers.CoC.Logic.Structure;
using Newtonsoft.Json.Linq;

namespace BL.Servers.CoC.Logic.Manager
{
    internal class GameObjectManager
    {
        public GameObjectManager(Level l)
        {
            this.Level                = l;
            this.GameObjects          = new List<List<GameObject>>();
            GameObjectRemoveList = new List<GameObject>();
            this.GameObjectsIndex     = new List<int>();
            for (int i = 0; i < 7; i++)
            {
                this.GameObjects.Add(new List<GameObject>());
                this.GameObjectsIndex.Add(0);
            }
            this.ComponentManager     = new ComponentManager(this.Level);
		}

        readonly ComponentManager ComponentManager;
        readonly List<GameObject> GameObjectRemoveList;
        readonly List<List<GameObject>> GameObjects;
        readonly List<int> GameObjectsIndex;
        readonly Level Level;

		public void AddGameObject(GameObject go)
        {
            go.GlobalId = GenerateGameObjectGlobalId(go);
            if (go.ClassId == 0)
            {
                var b = (Building) go;
                var bd = b.GetBuildingData();
                if (bd.IsWorkerBuilding())
                    this.Level.WorkerManager.IncreaseWorkerCount();
            }
            this.GameObjects[go.ClassId].Add(go);
        }

        public List<List<GameObject>> GetAllGameObjects() => this.GameObjects;

        public ComponentManager GetComponentManager() => this.ComponentManager;

        //public ObstacleManager GetObstacleManager() => m_vObstacleManager;

        public GameObject GetGameObjectByID(int id)
        {
            var classId = GlobalID.GetType(id) - 500;
            if (this.GameObjects.Capacity < classId)
                return null;
            return this.GameObjects[classId].Find(g => g.GlobalId == id);
        }

        public List<GameObject> GetGameObjects(int id) => this.GameObjects[id];

        public JObject JSON
        {
            get
            {
                #region Get
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
                    JObject j = new JObject { { "data", t.GetTrapData().GetGlobalID() }, { "id", 504000000 + u } };
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
                    Deco d = (Deco)go;
                    JObject j = new JObject { { "data", d.GetDecoData().GetGlobalID() }, { "id", 506000000 + e } };
                    d.Save(j);
                    JDecos.Add(j);
                    e++;
                }

                Player pl = this.Level.Avatar;
                var jsonData = new JObject
                {
                    {"exp_ver", 1},
                    {"android_client", true},
                    {"active_layout", 0 },
                    {"war_layout", 0 },
                    {"layout_state", new JArray {0, 0, 0, 0, 0, 0}},
                    {"buildings", JBuildings},
                    {"obstacles", JObstacles},
                    {"traps", JTraps},
                    {"decos", JDecos}
                };

                /*

                m_vObstacleManager.Save(jsonData); */

                var cooldowns = new JArray();
                jsonData.Add("cooldowns", cooldowns);
                var newShopBuildings = new JArray
                {
                    4, 0, 7, 4,
                    7, 4, 4, 1,
                    7, 8, 325, 5,
                    4, 4, 1, 5,
                    0, 0, 0, 4,
                    1, 4, 1, 3,
                    1, 1, 2, 2,
                    2, 1, 1, 1,
                    2
                };
                jsonData.Add("newShopBuildings", newShopBuildings);
                var newShopTraps = new JArray { 6, 6, 5, 0, 0, 5, 5, 0, 3 };
                jsonData.Add("newShopTraps", newShopTraps);
                var newShopDecos = new JArray
                {
                    100, 100, 100, 100,
                    100, 100, 100, 100,
                    100, 100, 100, 100,
                    100, 100, 100, 100,
                    100, 100, 100, 100,
                    100, 100, 100, 100,
                    100, 100, 100, 100,
                    100, 100, 100, 100,
                    100, 100, 100, 100,
                    100, 100
                };
                jsonData.Add("newShopDecos", newShopDecos);
                jsonData.Add("troop_req_msg", "Sup");
                jsonData.Add("last_league_rank", 11);
                jsonData.Add("last_league_shuffle", 1);
                jsonData.Add("last_season_seen", 1);
                jsonData.Add("last_news_seen", 999);
                jsonData.Add("edit_mode_shown", true);
                jsonData.Add("war_tutorials_seen", 1);
                jsonData.Add("war_base", true);
                jsonData.Add("help_opened", true);
                jsonData.Add("bool_layout_edit_shown_erase", false);

                return jsonData;
#endregion
            }
            set 
            {
                var jsonBuildings = (JArray) value["buildings"];
                foreach (JObject jsonBuilding in jsonBuildings)
                {
                    var bd =
                        CSV.Tables.Get(Gamefile.Buildings)
                            .GetDataWithID(jsonBuilding["data"].ToObject<int>()) as Buildings;
                    var b = new Building(bd, this.Level);
                    AddGameObject(b);
                    b.Load(jsonBuilding);
                }
                var jsonTraps = (JArray) value["traps"];
                foreach (JObject jsonTrap in jsonTraps)
                {
                    var td = CSV.Tables.Get(Gamefile.Traps).GetDataWithID(jsonTrap["data"].ToObject<int>()) as Traps;
                    var t = new Trap(td, this.Level);
                    AddGameObject(t);
                    t.Load(jsonTrap);
                }

                var jsonDecos = (JArray) value["decos"];
                foreach (JObject jsonDeco in jsonDecos)
                {
                    var dd = CSV.Tables.GetWithGlobalID(jsonDeco["data"].ToObject<int>()) as Decos;
                    var d = new Deco(dd, this.Level);
                    AddGameObject(d);
                    d.Load(jsonDeco);
                }

                var jsonObstacles = (JArray)value["obstacles"];
                foreach (JObject jsonObstacle in jsonObstacles)
                {
                    var dd = CSV.Tables.Get(Gamefile.Obstacles).GetDataWithID(jsonObstacle["data"].ToObject<int>()) as Obstacles;
                    var d = new Obstacle(dd, this.Level);
                    AddGameObject(d);
                    d.Load(jsonObstacle);
                }/*
    
                m_vObstacleManager.Load(jsonObject); */
            }
        }

        public void RemoveGameObject(GameObject go)
        {
            this.GameObjects[go.ClassId].Remove(go);
            if (go.ClassId == 0)
            {
                var b = (Building) go;
                var bd = b.GetBuildingData();
                if (bd.IsWorkerBuilding())
                {
                   this.Level.WorkerManager.DecreaseWorkerCount();
                }
            }
            RemoveGameObjectReferences(go);
        }

        public void RemoveGameObjectReferences(GameObject go)
        {
            this.ComponentManager.RemoveGameObjectReferences(go);
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
                var b = (Building) go;
                var bd = b.GetBuildingData();
                if (bd.IsWorkerBuilding())
                    this.Level.WorkerManager.DecreaseWorkerCount();
            }
            RemoveGameObjectReferences(go);
        }
    }
}
