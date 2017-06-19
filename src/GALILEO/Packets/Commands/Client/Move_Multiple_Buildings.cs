using System.Collections.Generic;
using System.Windows;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Logic;

namespace Republic.Magic.Packets.Commands.Client
{
    internal class Move_Multiple_Buildings : Command
    {
        internal List<BuildingToMove> Buildings;
        internal int Tick;

        public Move_Multiple_Buildings(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            int buildingCount = this.Reader.ReadInt32();
            this.Buildings = new List<BuildingToMove>(buildingCount);
            for (var i = 0; i < buildingCount; ++i)
            {
                this.Buildings.Add(new BuildingToMove()
                {
                    XY = new Vector(this.Reader.ReadInt32(), this.Reader.ReadInt32()),
                    Id = this.Reader.ReadInt32()
                });
            }
            this.Tick = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            foreach (var building in this.Buildings)
            {
                var go = this.Device.Player.Avatar.Variables.IsBuilderVillage ? this.Device.Player.GameObjectManager.GetBuilderVillageGameObjectByID(building.Id) : this.Device.Player.GameObjectManager.GetGameObjectByID(building.Id);
                go?.SetPositionXY(building.XY);
            }
        }
    }
}
