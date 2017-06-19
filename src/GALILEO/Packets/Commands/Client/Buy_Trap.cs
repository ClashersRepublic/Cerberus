using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Files;
using CRepublic.Magic.Files.CSV_Logic;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Logic.Structure;

namespace CRepublic.Magic.Packets.Commands.Client
{
    internal class Buy_Trap : Command
    {
        internal Vector Vector;
        internal int TrapID;
        internal int Tick;

        public Buy_Trap(Reader reader, Device client, int id) : base(reader, client, id)
        {
            this.Vector = new Vector();
        }

        internal override void Decode()
        {
            this.Vector.X = this.Reader.ReadInt32();
            this.Vector.Y = this.Reader.ReadInt32();
            this.TrapID = this.Reader.ReadInt32();
            this.Tick = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            var ca = this.Device.Player.Avatar;
            var td = (Traps) CSV.Tables.Get(Gamefile.Traps).GetDataWithID(this.TrapID);
            if (!ca.Variables.IsBuilderVillage)
            {
                var b = new Trap(td, this.Device.Player);

                if (ca.HasEnoughResources(td.GetBuildResource(0).GetGlobalID(), td.GetBuildCost(0)))
                {
                    if (this.Device.Player.Avatar.Variables.IsBuilderVillage ? this.Device.Player.HasFreeBuilderVillageWorkers : this.Device.Player.HasFreeVillageWorkers)
                    {
                        var rd = td.GetBuildResource(0);
                        ca.Resources.ResourceChangeHelper(rd.GetGlobalID(), -td.GetBuildCost(0));

                        b.StartConstructing(this.Vector, false);
                        this.Device.Player.GameObjectManager.AddGameObject(b);
                    }
                }
            }
            else
            {
                var b = new Builder_Trap(td, this.Device.Player);
                if (ca.HasEnoughResources(td.GetBuildResource(0).GetGlobalID(), td.GetBuildCost(0)))
                {
                    if (this.Device.Player.Avatar.Variables.IsBuilderVillage ? this.Device.Player.HasFreeBuilderVillageWorkers : this.Device.Player.HasFreeVillageWorkers)
                    {
                        var rd = td.GetBuildResource(0);
                        ca.Resources.ResourceChangeHelper(rd.GetGlobalID(), -td.GetBuildCost(0));

                        b.StartConstructing(this.Vector, true, true);
                        this.Device.Player.GameObjectManager.AddGameObject(b);
                    }
                }
            }
        }
    }
}
