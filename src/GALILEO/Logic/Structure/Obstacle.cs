using System;
using System.Collections.Generic;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Files;
using BL.Servers.CoC.Logic.Enums;

namespace BL.Servers.CoC.Logic.Structure
{
    using BL.Servers.CoC.Files.CSV_Helpers;
    using BL.Servers.CoC.Files.CSV_Logic;
    using Newtonsoft.Json.Linq;

    internal class Obstacle : GameObject
    {
        internal Level Level;
        internal Timer Timer;

        public Obstacle(Data data, Level l) : base(data, l)
        {
            this.Level = l;
        }

        internal override int ClassId => 3;

        internal Obstacles GetObstacleData() => (Obstacles)GetData();
        internal bool IsClearing;

        internal void CancelClearing()
        {
            this.Level.WorkerManager.DeallocateWorker(this);
            this.IsClearing = false;
            this.Timer = null;
            var od = GetObstacleData();
            this.Level.Avatar.Resources.ResourceChangeHelper(od.GetClearingResource().GetGlobalID(), od.ClearCost);
        }

        internal void StartClearing()
        {
            var constructionTime = GetObstacleData().ClearTimeSeconds;
            if (constructionTime < 1)
            {
                ClearingFinished();
            }
            else
            {
                this.Timer = new Timer();
                this.IsClearing = true;
                this.Timer.StartTimer(this.Level.Avatar.LastTick, constructionTime);
                this.Level.WorkerManager.AllocateWorker(this);
            }
        }

        internal void ClearingFinished()
        {
            this.Level.WorkerManager.DeallocateWorker(this);
            this.IsClearing = false;
            this.Timer = null;
            var constructionTime = GetObstacleData().ClearTimeSeconds;
            var exp = (int)Math.Pow(constructionTime, 0.5f);

            Level.Avatar.AddExperience(exp);

            var rd = CSV.Tables.Get(Gamefile.Resources).GetData(GetObstacleData().LootResource);

            Level.Avatar.Resources.ResourceChangeHelper(rd.GetGlobalID(), GetObstacleData().LootCount);

            Level.GameObjectManager.RemoveGameObject(this);
        }

        internal int GetRemainingClearingTime()
        {
            return this.Timer.GetRemainingSeconds(this.Level.Avatar.LastTick);
        }

        public new void Load(JObject jsonObject)
        {
            var remTimeToken = jsonObject["const_t"];
            var remTimeEndToken = jsonObject["const_t_end"];
            if (remTimeToken != null && remTimeEndToken != null)
            {
                this.Timer = new Timer();
                this.IsClearing = true;
                var remainingClearingTime = remTimeToken.ToObject<int>();
                this.Timer.StartTimer(this.Level.Avatar.LastTick, remainingClearingTime);
                this.Timer.EndTime = remTimeEndToken.ToObject<int>();
                this.Level.WorkerManager.AllocateWorker(this);
            }
            base.Load(jsonObject);
        }

        public new JObject Save(JObject jsonObject)
        {
            if (this.IsClearing)
            {
                jsonObject.Add("const_t", this.Timer.GetRemainingSeconds(this.Level.Avatar.LastTick));
                jsonObject.Add("const_t_end", this.Timer.EndTime);
            }
            base.Save(jsonObject);
            return jsonObject;
        }

        public override void Tick()
        {
            base.Tick();
            if (this.IsClearing)
            {
                if (this.Timer.GetRemainingSeconds(this.Level.Avatar.LastTick) <= 0)
                    ClearingFinished();
            }
        }
    }
}