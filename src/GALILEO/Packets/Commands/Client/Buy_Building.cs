using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Republic.Magic.Core;
using Republic.Magic.Extensions;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Files;
using Republic.Magic.Files.CSV_Logic;
using Republic.Magic.Logic;
using Republic.Magic.Logic.Enums;
using Republic.Magic.Logic.Structure;

namespace Republic.Magic.Packets.Commands.Client
{

    internal class Buy_Building : Command
    {
        internal int BuildingId;
        internal int Tick;
        internal Vector Vector;

        public Buy_Building(Reader reader, Device client, int id) : base(reader, client, id)
        {
            this.Vector = new Vector();
        }

        internal override void Decode()
        {
            this.Vector.X = this.Reader.ReadInt32();
            this.Vector.Y = this.Reader.ReadInt32();
            this.BuildingId = this.Reader.ReadInt32();
            this.Tick = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            var ca = this.Device.Player.Avatar;
            var bd = (Buildings) CSV.Tables.Get(Gamefile.Buildings).GetDataWithID(BuildingId);
            if (!ca.Variables.IsBuilderVillage)
            {
                var b = new Building(bd, this.Device.Player);

                if (ca.HasEnoughResources(bd.GetBuildResource(0).GetGlobalID(), bd.GetBuildCost(0)))
                {
                    if (bd.IsWorkerBuilding())
                    {
                        b.StartConstructing(this.Vector, false);
                        this.Device.Player.GameObjectManager.AddGameObject(b);
                        return;
                    }

                    if (this.Device.Player.HasFreeVillageWorkers)
                    {
                        var rd = bd.GetBuildResource(0);
                        ca.Resources.ResourceChangeHelper(rd.GetGlobalID(), -bd.GetBuildCost(0));

                        b.StartConstructing(this.Vector, this.Device.Player.Avatar.Variables.IsBuilderVillage);
                        this.Device.Player.GameObjectManager.AddGameObject(b);
                    }
                }
            }
            else
            {
                var b = new Builder_Building(bd, this.Device.Player);
                if (ca.HasEnoughResources(bd.GetBuildResource(0).GetGlobalID(), bd.GetBuildCost(0)))
                {
                    if (bd.IsWorker2Building())
                    {
                        b.StartConstructing(this.Vector, true);
                        this.Device.Player.GameObjectManager.AddGameObject(b);
                        return;
                    }

                    if (this.Device.Player.HasFreeBuilderVillageWorkers)
                    {
                        var rd = bd.GetBuildResource(0);
                        ca.Resources.ResourceChangeHelper(rd.GetGlobalID(), -bd.GetBuildCost(0));

                        b.StartConstructing(this.Vector, true);
                        this.Device.Player.GameObjectManager.AddGameObject(b);
                    }
                }
            }
        }
    }
}
