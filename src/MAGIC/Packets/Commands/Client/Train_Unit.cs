using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Files;
using BL.Servers.CoC.Files.CSV_Logic;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;

namespace BL.Servers.CoC.Packets.Commands.Client
{

    internal class Train_Unit : Command
    {
        public Train_Unit(Reader reader, Device client, int id) : base(reader, client, id)
        {

        }

        internal override void Decode()
        {
            this.Reader.ReadInt32();
            this.Reader.ReadUInt32();
            this.UnitType = this.Reader.ReadInt32();
            this.Count = this.Reader.ReadInt32();
            this.Reader.ReadUInt32();
            this.Tick = this.Reader.ReadInt32();
        }

        internal int Count;
        internal int UnitType;
        internal int Tick;

        internal override void Process()
        {
            Player _Player = this.Device.Player.Avatar;

            var _TroopData = CSV.Tables.Get(Gamefile.Characters).GetDataWithID(UnitType) as Combat_Item;
            _Player.Add_Unit(_TroopData.GetGlobalID(), Count);
        }
    }
}