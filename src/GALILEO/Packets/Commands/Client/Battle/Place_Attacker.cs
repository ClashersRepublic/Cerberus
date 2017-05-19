using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Extensions;
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

        internal Characters Troop;
        internal Spells Spell;

        internal int GlobalId;
        internal int X;
        internal int Y;
        internal int Tick;

        internal bool IsSpell;

        public Place_Attacker(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.X = this.Reader.ReadInt32();
            this.Y = this.Reader.ReadInt32();
            this.GlobalId = this.Reader.ReadInt32();
            if (GlobalId < 4000000)
            {
                this.IsSpell = true;
                this.Spell = CSV.Tables.Get(Gamefile.Spells).GetDataWithID(GlobalId) as Spells;
            }
            else
            {
                this.Troop = CSV.Tables.Get(Gamefile.Characters).GetDataWithID(GlobalId) as Characters;
            }
            this.Tick = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            if (this.Device.State == State.IN_PC_BATTLE)
            {
                var Battle = Core.Resources.Battles.Get(this.Device.Player.Avatar.Battle_ID, Constants.Database);
                Battle_Command Command = new Battle_Command { Command_Type = this.Identifier, Command_Base = new Command_Base { Base = new Base { Tick = this.Tick }, Data = this.GlobalId, X = this.X, Y = this.Y } };
                Battle.Add_Command(Command);

                if (IsSpell)
                {
                    int Index = Battle.Replay_Info.Spells.FindIndex(T => T[0] == this.GlobalId);
                    if (Index > -1)
                        Battle.Replay_Info.Spells[Index][1]++;
                    else
                        Battle.Replay_Info.Spells.Add(new[] { this.GlobalId, 1 });

                    Battle.Attacker.Add_Spells(GlobalId, 1);
                }
                else
                {
                    int Index = Battle.Replay_Info.Units.FindIndex(T => T[0] == this.GlobalId);
                    if (Index > -1)
                        Battle.Replay_Info.Units[Index][1]++;
                    else
                        Battle.Replay_Info.Units.Add(new[] { this.GlobalId, 1 });

                    Battle.Attacker.Add_Unit(GlobalId, 1);
                }
            }

            if (IsSpell)
            {
                List<Slot> _PlayerSpells= this.Device.Player.Avatar.Spells;

                Slot _DataSlot = _PlayerSpells.Find(t => t.Data == GlobalId);
                if (_DataSlot != null)
                {
                    _DataSlot.Count -= 1;
                }
            }
            else
            {
                List<Slot> _PlayerUnits= this.Device.Player.Avatar.Units;

                Slot _DataSlot = _PlayerUnits.Find(t => t.Data == GlobalId);
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
