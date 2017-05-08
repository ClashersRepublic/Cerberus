using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BL.Servers.CoC.Core;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Files;
using BL.Servers.CoC.Files.CSV_Logic;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Logic.Structure;

namespace BL.Servers.CoC.Packets.Commands.Client
{

    internal class Buy_Building : Command
    {
        internal int BuildingId;
        internal uint Unknown1;
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
            this.Unknown1 = this.Reader.ReadUInt32();
        }

        internal override void Process()
        {
            var ca = this.Device.Player.Avatar;
            var bd = (Buildings) CSV.Tables.Get(Gamefile.Buildings).GetDataWithID(BuildingId);
            var b = new Building(bd, this.Device.Player);

            if (ca.HasEnoughResources(bd.GetBuildResource(0).GetGlobalID(), bd.GetBuildCost(0)))
            {
                if (bd.IsWorkerBuilding() || this.Device.Player.HasFreeWorkers)
                {
                    var rd = bd.GetBuildResource(0);
                    ca.CommodityCountChangeHelper(0, rd, -bd.GetBuildCost(0));

                    b.StartConstructing(this.Vector);
                    this.Device.Player.GameObjectManager.AddGameObject(b);
                }
            }
        }
    }
}
