using System;
using BL.Servers.CoC.Logic.Manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BL.Servers.CoC.Logic
{
    internal class Level
    {
        internal Device Client;
        internal Player Avatar;

        internal GameObjectManager GameObjectManager;
        internal Village_Worker_Manager VillageWorkerManager;
        internal Builder_Village_Worker_Manager BuilderVillageWorkerManager;

        internal Level()
        {
            this.BuilderVillageWorkerManager = new Builder_Village_Worker_Manager();
            this.VillageWorkerManager = new Village_Worker_Manager();
            this.GameObjectManager = new GameObjectManager(this);
            this.Avatar = new Player();
        }

        internal Level(long id)
        {
            this.BuilderVillageWorkerManager = new Builder_Village_Worker_Manager();
            this.VillageWorkerManager = new Village_Worker_Manager();
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

        internal bool HasFreeVillageWorkers => this.VillageWorkerManager.GetFreeWorkers() > 0;
        internal bool HasFreeBuilderVillageWorkers => this.BuilderVillageWorkerManager.GetFreeWorkers() > 0;
    }
}
