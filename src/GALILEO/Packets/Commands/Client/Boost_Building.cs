using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Files.CSV_Logic;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Components;
using BL.Servers.CoC.Logic.Structure;
using Resource = BL.Servers.CoC.Logic.Enums.Resource;

namespace BL.Servers.CoC.Packets.Commands.Client
{
    internal class Boost_Building : Command
    {
        internal int IsBarrack;
        internal int Tick;

        public Boost_Building(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.IsBarrack = this.Reader.ReadInt32();
            this.Tick = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            var avatar = this.Device.Player.Avatar;
            if (this.IsBarrack == 1)
            {
                foreach (Unit_Production_Component c in this.Device.Player.GetComponentManager.GetComponents(3)
                    .ToList())
                {
                    if (!c.IsSpellForge)
                    {
                        if (c.GetParent.ClassId == 0)
                        { 
                            var Object = ((Building) c.GetParent);

                            if (Object != null)
                            {
                                int diamondCount =
                                    ((Buildings) Object.GetConstructionItemData()).BoostCost[Object.UpgradeLevel];
                                if (avatar.Resources.Gems >= diamondCount)
                                {
                                    Object.BoostBuilding();
                                    avatar.Resources.Minus(Resource.Diamonds, diamondCount);
                                }
                            }
                        }
                        else if (c.GetParent.ClassId == 7)
                        {
                            var Object = ((Builder_Building)c.GetParent);

                            if (Object != null)
                            {
                                int diamondCount =
                                    ((Buildings)Object.GetConstructionItemData()).BoostCost[Object.UpgradeLevel];
                                if (avatar.Resources.Gems >= diamondCount)
                                {
                                    Object.BoostBuilding();
                                    avatar.Resources.Minus(Resource.Diamonds, diamondCount);
                                }
                            }

                        }
                    }
                }

            }
            else if (this.IsBarrack == 0)
            {
                foreach (Unit_Production_Component c in this.Device.Player.GetComponentManager.GetComponents(3).ToList())
                {
                    if (c.IsSpellForge)
                    {
                        if (c.GetParent.ClassId == 0)
                        {
                            var Object = ((Building) c.GetParent);
                            if (Object != null)
                            {
                                int diamondCount =
                                    ((Buildings) Object.GetConstructionItemData()).BoostCost[Object.UpgradeLevel];
                                if (avatar.Resources.Gems >= diamondCount)
                                {
                                    Object.BoostBuilding();
                                    avatar.Resources.Minus(Resource.Diamonds, diamondCount);
                                }
                            }
                        }
                        else if (c.GetParent.ClassId == 7)
                        {
                            var Object = ((Builder_Building)c.GetParent);
                            if (Object != null)
                            {
                                int diamondCount =
                                    ((Buildings)Object.GetConstructionItemData()).BoostCost[Object.UpgradeLevel];
                                if (avatar.Resources.Gems >= diamondCount)
                                {
                                    Object.BoostBuilding();
                                    avatar.Resources.Minus(Resource.Diamonds, diamondCount);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
