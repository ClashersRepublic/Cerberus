using System;
using System.Collections.Generic;
namespace BL.Servers.BB.Logic.Structure
{
    using BL.Servers.BB.Files.CSV_Helpers;
    using BL.Servers.BB.Files.CSV_Logic;

    internal class Obstacle : GameObject
    {
        internal Level Level;

        public Obstacle(Data data, Level l) : base(data, l)
        {
            this.Level = l;
        }

        internal override int ClassId => 3;

        public Obstacles GetObstacleData() => (Obstacles)GetData();

    }
}