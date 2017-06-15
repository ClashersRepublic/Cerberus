using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Logic.Structure.Slots.Items;

namespace BL.Servers.CoC.Packets.Commands.Client.Battle
{
    internal class Hero_Rage : Command
    {

        internal int GlobalId;
        internal int Tick;

        public Hero_Rage(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.GlobalId = this.Reader.ReadInt32();
            this.Tick = this.Reader.ReadInt32();
        }
        internal override void Process()
        {
            if (this.Device.State == State.IN_PC_BATTLE)
            {
                if (!this.Device.Player.Avatar.Variables.IsBuilderVillage)
                {
                    var Battle = Core.Resources.Battles.Get(this.Device.Player.Avatar.Battle_ID, Constants.Database);
                    Battle_Command Command = new Battle_Command
                    {
                        Command_Type = this.Identifier,
                        Command_Base = new Command_Base {Base = { Tick = this.Tick }, Data = this.GlobalId}
                    };
                    Battle.Add_Command(Command);
                }
                else
                {
                    Battle_Command Command = new Battle_Command
                    {
                        Command_Type = this.Identifier,
                        Command_Base = new Command_Base { Base = { Tick = this.Tick }, Data = this.GlobalId }
                    };
                    this.Device.Player.Avatar.Battle_V2.Add_Command(Command);

                }
            }

        }

    }
}
