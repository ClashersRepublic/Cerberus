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
using BL.Servers.CoC.Logic.Structure.Slots.Items;

namespace BL.Servers.CoC.Packets.Commands.Client.Battle
{
    internal class Place_Attacker : Command
    {
        internal Combat_Item Unit;
        internal uint Tick;
        internal int X;
        internal int Y;

        public Place_Attacker(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }
        internal override void Decode()
        {
            this.X = this.Reader.ReadInt32();
            this.Y = this.Reader.ReadInt32();
            this.Unit = (Combat_Item)CSV.Tables.GetWithGlobalID(this.Reader.ReadInt32());
            this.Tick = this.Reader.ReadUInt32();
        }

        internal override void Process()
        {
            if (this.Device.State == State.IN_BATTLE)
            {
                List<Slot> _PlayerUnits = this.Device.Player.Avatar.Units;

                Slot _DataSlot = _PlayerUnits.Find(t => t.Data == Unit.GetGlobalID());
                if (_DataSlot != null)
                {
                    _DataSlot.Count -= 1;
                }
            }
            /*            List<Component> components = level.GetComponentManager().GetComponents(0);
            for (int i = 0; i < components.Count; i++)
            {
                UnitStorageComponent c = (UnitStorageComponent)components[i];
                if (c.GetUnitTypeIndex(Unit) != -1)
                {
                    var storageCount = c.GetUnitCountByData(Unit);
                    if (storageCount >= 1)
                    {
                        c.RemoveUnits(Unit, 1);
                        break;
                    }
                }
            }*/
        }
    }
}
