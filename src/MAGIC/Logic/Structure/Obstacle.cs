using System;
using System.Collections.Generic;
namespace BL.Servers.CoC.Logic.Structure
{
    using BL.Servers.CoC.Files.CSV_Helpers;
    using BL.Servers.CoC.Files.CSV_Logic;

    internal class Obstacle : GameObject
    {
        internal Level Level;

        public Obstacle(Data data, Level l) : base(data, l)
        {
            this.Level = l;
        }

        internal override int ClassId => 3;

        internal Obstacles GetObstacleData() => (Obstacles)GetData();

    }
}