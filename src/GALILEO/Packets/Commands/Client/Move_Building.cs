using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Structure;

namespace BL.Servers.CoC.Packets.Commands.Client
{
    internal class Move_Building : Command
    {
        internal int BuildingId;
        internal int Tick;
        internal Vector Position;
        internal int X;
        internal int Y;

        public Move_Building(Reader reader, Device client, int id) : base(reader, client, id)
        {
            this.Position = new Vector(0, 0);
        }
        internal override void Decode()
        {
            this.Position.X = this.Reader.ReadInt32();
            this.Position.Y = this.Reader.ReadInt32();
            this.BuildingId = this.Reader.ReadInt32();
            this.Tick = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            GameObject go = this.Device.Player.GameObjectManager.GetGameObjectByID(BuildingId);
            go.SetPositionXY(Position);
        }
    }
}