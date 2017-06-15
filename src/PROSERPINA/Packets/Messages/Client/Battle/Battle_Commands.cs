using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BL.Servers.CR.Core;
using BL.Servers.CR.Core.Network;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;
using BL.Servers.CR.Packets.Commands.Client.Battles;
using BL.Servers.CR.Packets.Messages.Server.Battle;

namespace BL.Servers.CR.Packets.Messages.Client.Battle
{
    internal class Battle_Commands : Message
    {
        public Battle_Commands(Device _Client, Reader Reader) : base(_Client, Reader)
        {
            // Battle_Commands.
        }
        internal int CommandID = 0;
        internal int CommandValue = 0;
        internal int CommandTick = 0;
        internal int CommandSum = 0;
        internal int CommandUnk = 0;
        internal int CommandUnk2 = 0;

        internal override void Decode()
        {
            this.CommandID = this.Reader.ReadVInt();
            this.Reader.ReadVInt();
            this.CommandSum = this.Reader.ReadVInt();
            this.CommandUnk = this.Reader.ReadVInt();
            this.CommandTick = this.Reader.ReadVInt();
            this.CommandUnk2 = this.Reader.ReadInt16();
            this.CommandValue = this.Reader.ReadVInt();
        }

        internal override void Process()
        {
            Thread.Sleep((int)TimeSpan.FromSeconds(5).TotalSeconds);

            Logic.Player _Enemy = Server_Resources.Battles.GetEnemy(this.Device.Player.BattleID, this.Device.Player.UserId);

            Server_Resources.Battles[this.Device.Player.BattleID].Tick = this.CommandTick;
            Server_Resources.Battles[this.Device.Player.BattleID].Checksum = this.CommandSum;

            if (this.CommandID == 1)
            {
                Server_Resources.Battles[this.Device.Player.BattleID].Commands.Enqueue(new Place_Troop(_Enemy.Device)
                {
                    SenderID = this.Device.Player.UserId,
                    Checksum = this.CommandSum,
                    TroopType = 27,
                    TroopID = 33,
                    Tick = this.CommandTick,
                    X = 15000,
                    Y = 15000
                });

                new Battle_Command_Data(this.Device)
                {
                    Battle = Server_Resources.Battles[this.Device.Player.BattleID]
                }.Send();

                new Battle_Command_Data(_Enemy.Device)
                {   
                    Battle = Server_Resources.Battles[_Enemy.Decks.Player.BattleID]
                }.Send();
            }
            else if (this.CommandID == 3)
            {
                new Battle_Event_Data(_Enemy.Device)
                {
                    CommandID = this.CommandID,
                    CommandValue = this.CommandValue,
                    CommandTick = this.CommandTick,
                    CommandUnk = this.CommandUnk,
                    CommandUnk2 = this.CommandUnk2,
                    CommandSenderHigh = this.Device.Player.UserHighId,
                    CommandSenderLow = this.Device.Player.UserLowId,
                }.Send();
            }
        }
    }
}
