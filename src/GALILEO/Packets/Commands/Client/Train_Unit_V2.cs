using System;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Files;
using BL.Servers.CoC.Files.CSV_Logic;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;

namespace BL.Servers.CoC.Packets.Commands.Client
{
    internal class Train_Unit_V2 : Command
    {
        internal int BuildingID;
        internal int Unknown1;
        internal Characters Unit;
        internal int Tick;
        public Train_Unit_V2(Reader reader, Device client, int id) : base(reader, client, id)
        {

        }

        internal override void Decode()
        {
            this.BuildingID = this.Reader.ReadInt32();
            this.Unknown1 = this.Reader.ReadInt32();
            this.Unit = CSV.Tables.Get(Gamefile.Characters).GetDataWithID(this.Reader.ReadInt32()) as Characters;
            this.Tick = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            ShowValues();
            Console.WriteLine($"Unit name {this.Unit.Name}");
            Console.WriteLine($"Unit cost {this.Unit.TrainingCost[this.Device.Player.Avatar.GetUnitUpgradeLevel(this.Unit)]}");
            Console.WriteLine($"Unit resource {this.Unit.TrainingResource}");
            Console.WriteLine($"Unit count in camp {this.Unit.UnitsInCamp[this.Device.Player.Avatar.GetUnitUpgradeLevel(this.Unit)]}");

        }
    }
}
