namespace CRepublic.Magic.Logic
{
    internal class Level
    {
        internal Device Device;
        internal Player Avatar; 

        internal Level()
        {
            Avatar = new Player();
        }

        internal Level(long id)
        {
           Avatar = new Player(id);
        }

        /*internal string JSON
        {
            get => JsonConvert.SerializeObject(GameObjectManager.JSON, Formatting.Indented);
            set => this.GameObjectManager.JSON = JObject.Parse(value);
        }

        internal void Reset()
        {
            var gameObjects = GameObjectManager.GetAllGameObjects();
            foreach (List<GameObject> t in gameObjects)
                t.Clear();
        }

        internal void Tick()
        {
            this.Avatar.LastTick = DateTime.UtcNow;
            GameObjectManager.Tick();
        }

        internal ComponentManager GetComponentManager => this.GameObjectManager.GetComponentManager();

        internal bool HasFreeVillageWorkers => this.VillageWorkerManager.GetFreeWorkers() > 0;
        internal bool HasFreeBuilderVillageWorkers => this.BuilderVillageWorkerManager.GetFreeWorkers() > 0;*/
    }
}
