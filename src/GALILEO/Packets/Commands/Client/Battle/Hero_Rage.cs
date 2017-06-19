using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Logic.Structure.Slots.Items;

namespace CRepublic.Magic.Packets.Commands.Client.Battle
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
                    var Battle = Core.Resources.Battles_V2.GetPlayer(this.Device.Player.Avatar.Battle_ID_V2, this.Device.Player.Avatar.UserId);

                    Battle_Command Command = new Battle_Command
                    {
                        Command_Type = this.Identifier,
                        Command_Base = new Command_Base { Base = { Tick = this.Tick }, Data = this.GlobalId }
                    };
                     Battle.Add_Command(Command);

                }
            }

        }

    }
}
