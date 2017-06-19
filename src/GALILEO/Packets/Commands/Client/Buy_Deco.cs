using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Files;
using Republic.Magic.Files.CSV_Logic;
using Republic.Magic.Logic;
using Republic.Magic.Logic.Enums;
using Republic.Magic.Logic.Structure;

namespace Republic.Magic.Packets.Commands.Client
{
    internal class Buy_Deco : Command
    {
        internal int BuildingId;
        internal int Tick;
        internal Vector Vector;

        public Buy_Deco(Reader reader, Device client, int id) : base(reader, client, id)
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
            var bd = (Decos)CSV.Tables.Get(Gamefile.Decos).GetDataWithID(BuildingId);
            if (!ca.Variables.IsBuilderVillage)
            {
                var b = new Deco(bd, this.Device.Player);

                if (ca.HasEnoughResources(bd.GetBuildResource().GetGlobalID(), bd.GetBuildCost()))
                {
                    ca.Resources.ResourceChangeHelper(bd.GetGlobalID(), -bd.GetBuildCost());
                    b.SetPositionXY(this.Vector);
                    this.Device.Player.GameObjectManager.AddGameObject(b);
                }
            }
            else
            {
                var b = new Builder_Deco(bd, this.Device.Player);
                if (ca.HasEnoughResources(bd.GetBuildResource().GetGlobalID(), bd.GetBuildCost()))
                {
                    ca.Resources.ResourceChangeHelper(bd.GetGlobalID(), -bd.GetBuildCost());
                    b.SetPositionXY(this.Vector);
                    this.Device.Player.GameObjectManager.AddGameObject(b);
                }
            }
        }
    }
}
