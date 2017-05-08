using System;
using System.Collections.Generic;
using System.Windows;
using BL.Servers.BB.Core.Network;
using BL.Servers.BB.Extensions.List;
using BL.Servers.BB.Logic.Structure;
using BL.Servers.BB.Packets.Messages.Server;

namespace BL.Servers.BB.Packets.Commands.Client
{
    using BL.Servers.BB.Extensions.Binary;
    using BL.Servers.BB.Files;
    using BL.Servers.BB.Files.CSV_Logic;
    using BL.Servers.BB.Logic.Enums;
    using BL.Servers.BB.Logic;

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
