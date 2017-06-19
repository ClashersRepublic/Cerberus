using System;
using System.Collections.Generic;
using System.Windows;
using CRepublic.Boom.Core.Network;
using CRepublic.Boom.Extensions.List;
using CRepublic.Boom.Logic.Structure;
using CRepublic.Boom.Packets.Messages.Server;

namespace CRepublic.Boom.Packets.Commands.Client
{
    using CRepublic.Boom.Extensions.Binary;
    using CRepublic.Boom.Files;
    using CRepublic.Boom.Files.CSV_Logic;
    using CRepublic.Boom.Logic.Enums;
    using CRepublic.Boom.Logic;

    internal class Buy_Building : Command
    {
        internal int BuildingData;
        internal Vector Positon;

        public Buy_Building(Reader _Reader, Device _Client, int _ID) : base(_Reader, _Client, _ID)
        {
            this.Positon = new Vector(0, 0);
        }

        internal override void Decode()
        {
            this.Positon.X = this.Reader.ReadInt32();
            this.Positon.Y = this.Reader.ReadInt32();

            this.BuildingData = this.Reader.ReadInt32();

            this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            Buildings data = CSV.Tables.Get(Gamefile.Buildings).GetDataWithID(this.BuildingData) as Buildings;
            Console.WriteLine($"Constructing Building: {data.Name} (X : {this.Positon.X}, Y : {this.Positon.Y})");

            var ca = this.Device.Player.Avatar;
            var bd = CSV.Tables.Get(Gamefile.Buildings).GetDataWithID(this.BuildingData) as Buildings;
            var b = new Building(bd, this.Device.Player);
            b.StartConstructing(this.Positon);
            this.Device.Player.GameObjectManager.AddGameObject(b);
        }
    }
}
