using System;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Logic.Structure.Slots.Items;
using BL.Servers.CoC.Packets.Messages.Server.Battle;

namespace BL.Servers.CoC.Packets.Commands.Client.Battle
{
    internal class Surrender_Attack : Command
    {
        internal int Tick;

        public Surrender_Attack(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
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
                        Command_Base = new Command_Base { Tick = this.Tick }
                    };
                    Battle.Add_Command(Command);
                }
                else
                {

                    new V2_Battle_Result(this.Device).Send();
                    Battle_Command Command = new Battle_Command
                    {
                        Command_Type = this.Identifier,
                        Command_Base = new Command_Base { Tick = this.Tick }
                    };
                    this.Device.Player.Avatar.Battle_V2.Add_Command(Command);

                }
            }

        }
    }
}
