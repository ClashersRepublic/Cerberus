using System;
using CRepublic.Magic.Core;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Files;
using CRepublic.Magic.Files.CSV_Logic;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Logic.Structure;
using Newtonsoft.Json.Linq;

namespace CRepublic.Magic.Logic.Components
{
    internal class Resource_Production_Component : Component
    {
        public Resource_Production_Component(ConstructionItem ci, Level level) : base(ci)
        {
            this.TimeSinceLastClick = level.Avatar.LastTick;
            this.ProductionResourceData = CSV.Tables.Get(Gamefile.Resources).GetData(((Buildings) ci.GetData()).ProducesResource) as Files.CSV_Logic.Resource;
            this.ResourcesPerHour = ((Buildings)ci.GetData()).ResourcePer100Hours;
            this.MaxResources = ((Buildings) ci.GetData()).ResourceMax;
        }

        internal override int Type => 5;

        internal int[] MaxResources;
        internal Files.CSV_Logic.Resource ProductionResourceData;
        internal int[] ResourcesPerHour;
        internal DateTime TimeSinceLastClick;

        internal void CollectResources()
        {
            var ci = (ConstructionItem)GetParent;
            var span = ci.Level.Avatar.LastTick - this.TimeSinceLastClick;
            float currentResources = 0;
            if (!ci.IsBoosted)
            {
                currentResources = (float)(this.ResourcesPerHour[ci.UpgradeLevel] / 100.0) / (60f * 60f) * (float)span.TotalSeconds;
            }
            else
            {
                if (ci.GetBoostEndTime >= ci.Level.Avatar.LastTick)
                {
                    currentResources = (float)(this.ResourcesPerHour[ci.UpgradeLevel] / 100.0) / (60f * 60f) * (float)span.TotalSeconds;
                    currentResources *= ci.GetBoostMultipier();
                }
                else
                {
                    var boostedTime = (float)span.TotalSeconds - (float)(ci.Level.Avatar.LastTick - ci.GetBoostEndTime).TotalSeconds;
                    var notBoostedTime = (float)span.TotalSeconds - boostedTime;
                    currentResources = (float)(this.ResourcesPerHour[ci.UpgradeLevel] / 100.0) / (60f * 60f) * notBoostedTime;
                    currentResources += (float)(this.ResourcesPerHour[ci.UpgradeLevel] / 100.0) / (60f * 60f) * boostedTime * ci.GetBoostMultipier();
                    ci.IsBoosted = false;
                }
            }

            currentResources = Math.Min(Math.Max(currentResources, 0), this.MaxResources[ci.UpgradeLevel]);

            if (currentResources >= 1)
            {
                var ca = ci.Level.Avatar;
                if (ca.Resources_Cap.Get(this.ProductionResourceData.GetGlobalID()) >= ca.Resources.Get(this.ProductionResourceData.GetGlobalID()) || this.ProductionResourceData.GetGlobalID() == 3000000)
                {
                    if (this.ProductionResourceData.GetGlobalID() != 3000000)
                    {
                        if (ca.Resources_Cap.Get(this.ProductionResourceData.GetGlobalID()) -
                            ca.Resources.Get(this.ProductionResourceData.GetGlobalID()) < currentResources)
                        {
                            var newCurrentResources =
                                ca.Resources_Cap.Get(this.ProductionResourceData.GetGlobalID() -
                                                     ca.Resources.Get(this.ProductionResourceData.GetGlobalID()));
                            this.TimeSinceLastClick =
                                ci.Level.Avatar.LastTick.AddSeconds(-((currentResources - newCurrentResources) /
                                                                      ((float) (this.ResourcesPerHour[ci.UpgradeLevel] /
                                                                                100.0) / (60f * 60f))));
                            currentResources = newCurrentResources;
                        }
                        else
                        {
                            this.TimeSinceLastClick = ci.Level.Avatar.LastTick;
                        }
                    }
                    else
                    {
                        this.TimeSinceLastClick = ci.Level.Avatar.LastTick;
                    }
#if DEBUG
                    Loggers.Log($"Resource System : Collecting {currentResources} of {this.ProductionResourceData.Name}", true, Defcon.INFO);
#endif
                    ca.Resources.Plus(this.ProductionResourceData.GetGlobalID(), (int)currentResources);
                }
            }
        }

        internal override void Load(JObject jsonObject)
        {
            var productionObject = (JObject)jsonObject["production"];
            if (productionObject != null)
            {
                this.TimeSinceLastClick = TimeUtils.FromUnixTimestamp(productionObject["t_lastClick"].ToObject<int>());
            }
        }

        internal void Reset()
        {
            this.TimeSinceLastClick = GetParent.Level.Avatar.LastTick;
        }

        internal override JObject Save(JObject jsonObject)
        {
            if (((ConstructionItem)GetParent).GetUpgradeLevel() != -1)
            {
                var productionObject = new JObject {{"t_lastClick", (int)TimeUtils.ToUnixTimestamp(this.TimeSinceLastClick)} };

                jsonObject.Add("production", productionObject);
                var ci = (ConstructionItem)GetParent;
                var seconds = (float)(GetParent.Level.Avatar.LastTick - TimeSinceLastClick).TotalSeconds;
                
                if (ci.IsBoosted)
                {
                    if (ci.GetBoostEndTime >= ci.Level.Avatar.LastTick)
                    {
                        seconds *= ci.GetBoostMultipier();
                    }
                    else
                    {
                        var boostedTime = seconds - (float)(ci.Level.Avatar.LastTick - ci.GetBoostEndTime).TotalSeconds;
                        var notBoostedTime = seconds - boostedTime;
                        seconds = boostedTime * ci.GetBoostMultipier() + notBoostedTime;
                    }
                }
                jsonObject.Add("res_time", (int)((this.MaxResources[ci.GetUpgradeLevel()] / (float)(this.ResourcesPerHour[ci.UpgradeLevel] / 100.0) * 3600f) - seconds));
            }
            return jsonObject;
        }
    }
}
