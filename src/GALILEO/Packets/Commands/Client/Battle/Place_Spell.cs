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
    internal class Place_Spell : Command
    {
        internal int GlobalId;
        internal int X;
        internal int Y;
        internal byte Unknown_Byte;
        internal int Unknown_Int;
        internal int Tick;

        internal Spells Spell;

        public Place_Spell(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.X = this.Reader.ReadInt32();
            this.Y = this.Reader.ReadInt32();
            this.GlobalId = this.Reader.ReadInt32();
            this.Spell = CSV.Tables.Get(Gamefile.Spells).GetDataWithID(GlobalId) as Spells;
            this.Unknown_Byte = this.Reader.ReadByte();
            this.Unknown_Int = this.Reader.ReadInt32();
            this.Tick = this.Reader.ReadInt32();
        }

        internal override async void Process()
        {
            if (this.Device.State == State.IN_PC_BATTLE)
            {
                if (!this.Device.Player.Avatar.Variables.IsBuilderVillage)
                {
                    var Battle = await Core.Resources.Battles.Get(this.Device.Player.Avatar.Battle_ID, Constants.Database);
                    Battle_Command Command =
                        new Battle_Command
                        {
                            Command_Type = this.Identifier,
                            Command_Base =
                                new Command_Base
                                {
                                    Base = new Base {Tick = this.Tick},
                                    Data = this.GlobalId,
                                    X = this.X,
                                    Y = this.Y
                                }
                        };
                    Battle.Add_Command(Command);

                    int Index = Battle.Replay_Info.Spells.FindIndex(T => T[0] == this.GlobalId);
                    if (Index > -1)
                        Battle.Replay_Info.Spells[Index][1]++;
                    else
                        Battle.Replay_Info.Spells.Add(new[] {this.GlobalId, 1});

                    Battle.Attacker.Add_Spells(GlobalId, 1);

                }
                else
                {

                    Battle_Command Command =
                        new Battle_Command
                        {
                            Command_Type = this.Identifier,
                            Command_Base =
                                new Command_Base
                                {
                                    Base = new Base {Tick = this.Tick},
                                    Data = this.GlobalId,
                                    X = this.X,
                                    Y = this.Y
                                }
                        };
                    this.Device.Player.Avatar.Battle_V2.Add_Command(Command);
                }
            }
            List<Slot> _PlayerSpells = this.Device.Player.Avatar.Spells;

            Slot _DataSlot = _PlayerSpells.Find(t => t.Data == GlobalId);
            if (_DataSlot != null)
            {
                _DataSlot.Count -= 1;
            }
        }
    }
}
