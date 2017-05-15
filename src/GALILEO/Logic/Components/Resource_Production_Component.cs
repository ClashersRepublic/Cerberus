/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Files;
using BL.Servers.CoC.Files.CSV_Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Logic.Structure;
using BL.Servers.CoC.Logic.Structure.Slots;

namespace BL.Servers.CoC.Logic.Components
{
    internal class Resource_Production_Component : Component
    {
        public Resource_Production_Component(ConstructionItem ci, Level level) : base(ci)
        {
            this.TimeSinceLastClick = level.Avatar.LastTick;
            this.ProductionResourceData = CSV.Tables.Get(Gamefile.Resources).GetData(((Buildings) ci.GetData()).ProducesResource) as Files.CSV_Logic.Resource;
            this.ResourcesPerHour = ((Buildings)ci.GetData()).ResourcePerHour;
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
            var span = ci.Avatar.Avatar.LastTick - this.TimeSinceLastClick;
            float currentResources = 0;
            if (!ci.IsBoosted)
            {
                currentResources = this.ResourcesPerHour[ci.UpgradeLevel] / (60f * 60f) * (float)span.TotalSeconds;
            }
            else
            {
                if (ci.GetBoostEndTime() >= ci.Avatar.Avatar.LastTick)
                {
                    currentResources = m_vResourcesPerHour[ci.UpgradeLevel] / (60f * 60f) * (float)span.TotalSeconds;
                    currentResources *= ci.GetBoostMultipier();
                }
                else
                {
                    var boostedTime = (float)span.TotalSeconds - (float)(ci.Avatar.Avatar.LastTick - ci.GetBoostEndTime()).TotalSeconds;
                    var notBoostedTime = (float)span.TotalSeconds - boostedTime;
                    currentResources = m_vResourcesPerHour[ci.UpgradeLevel] / (60f * 60f) * notBoostedTime;
                    currentResources += m_vResourcesPerHour[ci.UpgradeLevel] / (60f * 60f) * boostedTime * ci.GetBoostMultipier();
                    ci.IsBoosted = false;
                }
            }

            currentResources = Math.Min(Math.Max(currentResources, 0), m_vMaxResources[ci.UpgradeLevel]);

            if (currentResources >= 1)
            {
                var ca = ci.Avatar.Avatar;
                if (ca.GetResourceCap(m_vProductionResourceData) >= ca.GetResourceCount(m_vProductionResourceData))
                {
                    if (ca.GetResourceCap(m_vProductionResourceData) - ca.GetResourceCount(m_vProductionResourceData) <
                        currentResources)
                    {
                        var newCurrentResources = ca.GetResourceCap(m_vProductionResourceData) - ca.GetResourceCount(m_vProductionResourceData);
                        this.TimeSinceLastClick = ci.Avatar
                            .Avatar.LastTickSaved
                            .AddSeconds(-((currentResources - newCurrentResources) / (m_vResourcesPerHour[ci.UpgradeLevel] / (60f * 60f))));
                        currentResources = newCurrentResources;
                    }
                    else
                    {
                        this.TimeSinceLastClick = ci.Avatar.Avatar.LastTick;
                    }

                    ca.CommodityCountChangeHelper(0, m_vProductionResourceData, (int)currentResources);
                }
            }
        }
    }
}*/
