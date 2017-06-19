using CRepublic.Magic.Extensions.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CRepublic.Magic.Files;
using CRepublic.Magic.Files.CSV_Logic;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Components;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Logic.Structure;

namespace CRepublic.Magic.Packets.Commands.Client
{
    internal class Buy_Multiple_Wall : Command
    {
        internal List<Vector> WallXYs;
        internal int WallID;
        internal int Count;
        internal int Tick;
        public Buy_Multiple_Wall(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.WallXYs = new List<Vector>(this.Count = this.Reader.ReadInt32());

            for (int i = 0; i < this.Count; i++)
            {
                this.WallXYs.Add(new Vector(this.Reader.ReadInt32(), this.Reader.ReadInt32()));
            }
            this.WallID = this.Reader.ReadInt32();
            this.Tick = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            this.Device.Player.Avatar.Wall_Group_ID++;
            foreach (var WallXY in this.WallXYs)
            {
                var bd = (Buildings)CSV.Tables.Get(Gamefile.Buildings).GetDataWithID(this.WallID);
                if (!this.Device.Player.Avatar.Variables.IsBuilderVillage)
                {
                    var b = new Building(bd, this.Device.Player);

                    if (this.Device.Player.Avatar.HasEnoughResources(bd.GetBuildResource(0).GetGlobalID(), bd.GetBuildCost(0)))
                    {
                        if (this.Device.Player.HasFreeVillageWorkers)
                        {
                            var rd = bd.GetBuildResource(0);
                            this.Device.Player.Avatar.Resources.ResourceChangeHelper(rd.GetGlobalID(), -bd.GetBuildCost(0));

                            var a = (Combat_Component) b.GetComponent(1, false);
                            a.WallI = this.Device.Player.Avatar.Wall_Group_ID;
                            b.StartConstructing(WallXY, this.Device.Player.Avatar.Variables.IsBuilderVillage);
                            this.Device.Player.GameObjectManager.AddGameObject(b);
                        }
                    }
                }
                else
                {
                    var b = new Builder_Building(bd, this.Device.Player);
                    if (this.Device.Player.Avatar.HasEnoughResources(bd.GetBuildResource(0).GetGlobalID(), bd.GetBuildCost(0)))
                    {
                        if (this.Device.Player.HasFreeBuilderVillageWorkers)
                        {
                            var rd = bd.GetBuildResource(0);
                            this.Device.Player.Avatar.Resources.ResourceChangeHelper(rd.GetGlobalID(), -bd.GetBuildCost(0));
                            var a = (Combat_Component)b.GetComponent(1, false);
                            a.WallI = this.Device.Player.Avatar.Wall_Group_ID;
                            b.StartConstructing(WallXY, this.Device.Player.Avatar.Variables.IsBuilderVillage);
                            this.Device.Player.GameObjectManager.AddGameObject(b);
                        }
                    }
                }
            }
        }
    }
}
