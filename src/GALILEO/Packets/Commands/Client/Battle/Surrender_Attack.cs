using System;
using CRepublic.Magic.Core;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Logic.Structure.Slots.Items;
using CRepublic.Magic.Packets.Messages.Server.Battle;

namespace CRepublic.Magic.Packets.Commands.Client.Battle
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
                    var Battle = Resources.Battles_V2.GetPlayer(this.Device.Player.Avatar.Battle_ID_V2, this.Device.Player.Avatar.UserId);
                    Battle.Set_Replay_Info();
                    new V2_Battle_Result(this.Device).Send();
                    Battle_Command Command = new Battle_Command
                    {
                        Command_Type = this.Identifier,
                        Command_Base = new Command_Base { Tick = this.Tick }
                    };
                    Battle.Add_Command(Command);

                }
            }

        }
    }
}
