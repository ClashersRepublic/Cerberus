using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Magic.ClashOfClans.Core;
using Magic.Files.Logic;

namespace Magic.ClashOfClans.Logic
{
    internal class ResourceProductionComponent : Component
    {
        public ResourceProductionComponent(ConstructionItem ci, Level level) : base(ci)
        {
            m_vTimeSinceLastClick = level.Time;
            m_vProductionResourceData =
           CsvManager.DataTables.GetResourceByName(((BuildingData) ci.Data).ProducesResource);
            m_vResourcesPerHour = ((BuildingData) ci.Data).ResourcePerHour;
            m_vMaxResources = ((BuildingData) ci.Data).ResourceMax;
        }

        public override int Type => 5;

        readonly List<int> m_vMaxResources;
        readonly ResourceData m_vProductionResourceData;
        readonly List<int> m_vResourcesPerHour;
        DateTime m_vTimeSinceLastClick;

        public void CollectResources()
        {
            var ci = (ConstructionItem) Parent;
            var span = ci.Level.Time- m_vTimeSinceLastClick;
            float currentResources = 0;
            if (!ci.IsBoosted)
            {
                currentResources = m_vResourcesPerHour[ci.UpgradeLevel] / (60f * 60f) * (float) span.TotalSeconds;
            }
            else
            {
                if (ci.GetBoostEndTime() >= ci.Level.Time)
                {
                    currentResources = m_vResourcesPerHour[ci.UpgradeLevel] / (60f * 60f) * (float) span.TotalSeconds;
                    currentResources *= ci.GetBoostMultipier();
                }
                else
                {
                    var boostedTime = (float) span.TotalSeconds - (float) (ci.Level.Time- ci.GetBoostEndTime()).TotalSeconds;
                    var notBoostedTime = (float) span.TotalSeconds - boostedTime;
                    currentResources = m_vResourcesPerHour[ci.UpgradeLevel] / (60f * 60f) * notBoostedTime;
                    currentResources += m_vResourcesPerHour[ci.UpgradeLevel] / (60f * 60f) * boostedTime * ci.GetBoostMultipier();
                    ci.IsBoosted = false;
                }
            }

            currentResources = Math.Min(Math.Max(currentResources, 0), m_vMaxResources[ci.UpgradeLevel]);

            if (currentResources >= 1)
            {
                var ca = ci.Level.Avatar;
                if (ca.GetResourceCap(m_vProductionResourceData) >= ca.GetResourceCount(m_vProductionResourceData))
                {
                    if (ca.GetResourceCap(m_vProductionResourceData) - ca.GetResourceCount(m_vProductionResourceData) <
                        currentResources)
                    {
                        var newCurrentResources = ca.GetResourceCap(m_vProductionResourceData) - ca.GetResourceCount(m_vProductionResourceData);
                        m_vTimeSinceLastClick = ci.Level                              .Time                              .AddSeconds(-((currentResources - newCurrentResources) / (m_vResourcesPerHour[ci.UpgradeLevel] / (60f * 60f))));
                        currentResources = newCurrentResources;
                    }
                    else
                    {
                        m_vTimeSinceLastClick = ci.Level.Time;
                    }

                    ca.CommodityCountChangeHelper(0, m_vProductionResourceData, (int) currentResources);
                }
            }
        }

        public override void Load(JObject jsonObject)
        {
            var productionObject = (JObject) jsonObject["production"];
            if (productionObject != null)
            {
                m_vTimeSinceLastClick = productionObject["t_lastClick"].ToObject<DateTime>();
            }
        }

        public void Reset()
        {
            m_vTimeSinceLastClick = Parent.Level.Time;
        }

        public override JObject Save(JObject jsonObject)
        {
            if (((ConstructionItem) Parent).GetUpgradeLevel() != -1)
            {
                var productionObject = new JObject();
                productionObject.Add("t_lastClick", m_vTimeSinceLastClick);
                jsonObject.Add("production", productionObject);
                var ci = (ConstructionItem) Parent;
                var seconds = (float) (Parent.Level.Time- m_vTimeSinceLastClick).TotalSeconds;
                if (ci.IsBoosted)
                {
                    if (ci.GetBoostEndTime() >= ci.Level.Time)
                    {
                        seconds *= ci.GetBoostMultipier();
                    }
                    else
                    {
                        var boostedTime = seconds - (float) (ci.Level.Time- ci.GetBoostEndTime()).TotalSeconds;
                        var notBoostedTime = seconds - boostedTime;
                        seconds = boostedTime * ci.GetBoostMultipier() + notBoostedTime;
                    }
                }
                jsonObject.Add("res_time", (int)
                        (m_vMaxResources[ci.GetUpgradeLevel()] / (float) m_vResourcesPerHour[ci.GetUpgradeLevel()] * 3600f -
                         seconds));
            }
            return jsonObject;
        }
    }
}
