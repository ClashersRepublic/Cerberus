using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic.Manager;
using Magic.ClashOfClans.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Magic.ClashOfClans.Logic
{
    internal class Level
    {
        public GameObjectManager GameObjectManager;
        public WorkerManager WorkerManager;

        private DateTime _timer;
        private Client _client;
        private byte _privilege;
        private byte _status;
        private string _ipAddress;
        private readonly Avatar _avatar;

        public Level()
        {
            WorkerManager = new WorkerManager();
            GameObjectManager = new GameObjectManager(this);

            _avatar = new Avatar();
            _privilege = 0;
            _status = 0;
            _ipAddress = "0.0.0.0";
        }

        public Level(long id, string token)
        {
            WorkerManager = new WorkerManager();
            GameObjectManager = new GameObjectManager(this);

            _avatar = new Avatar(id, token);
            _timer = DateTime.UtcNow;
            _privilege = 0;
            _status = 0;
            _ipAddress = "0.0.0.0";
        }

        public Avatar Avatar => _avatar;
        public DateTime Time { get; set; }
        public string IPAddress { get; set; }
        public byte AccountPrivileges { get; set; }
        public byte AccountStatus { get; set; }
        public Client Client { get; set; }

        public bool Banned => _status > 99;

        public ComponentManager GetComponentManager() => GameObjectManager.GetComponentManager();

        [Obsolete]
        public Avatar GetHomeOwnerAvatar() => _avatar;


        public bool HasFreeWorkers() => WorkerManager.GetFreeWorkers() > 0;

        public void LoadFromJson(string jsonString)
        {
            JObject jsonObject = JObject.Parse(jsonString);
            GameObjectManager.Load(jsonObject);
        }

        public string SaveToJson()
        {
            return JsonConvert.SerializeObject(GameObjectManager.Save());
        }

        public void SetHome(string jsonHome)
        {
            var gameObjects = GameObjectManager.GetAllGameObjects();
            for (int i = 0; i < gameObjects.Count; i++)
                gameObjects[i].Clear();

            GameObjectManager.Load(JObject.Parse(jsonHome));
        }

        public void Tick()
        {
            Time = DateTime.UtcNow;
            GameObjectManager.Tick();
        }
    }
}
