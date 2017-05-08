using System;
using BL.Servers.CoC.Logic.Manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UCS.Logic.Manager;

namespace BL.Servers.CoC.Logic
{
    internal class Level
    {
        internal Device Client;
        internal Player Avatar;

        internal GameObjectManager GameObjectManager;
        internal WorkerManager WorkerManager;

        internal Level()
        {
            this.WorkerManager = new WorkerManager();
            this.GameObjectManager = new GameObjectManager(this);
            this.Avatar = new Player();
        }

        internal Level(long id)
        {
            this.WorkerManager = new WorkerManager();
            this.GameObjectManager = new GameObjectManager(this);
            this.Avatar = new Player(id);
        }

        internal string JSON
        {
            get => JsonConvert.SerializeObject(GameObjectManager.JSON, Formatting.Indented);
            set => this.GameObjectManager.JSON = JObject.Parse(value);
        }

        internal void Tick()
        {
            this.Avatar.LastTick = DateTime.UtcNow;
            GameObjectManager.Tick();
        }

        internal ComponentManager GetComponentManager => this.GameObjectManager.GetComponentManager();

        internal bool HasFreeWorkers => this.WorkerManager.GetFreeWorkers() > 0;
    }
}
