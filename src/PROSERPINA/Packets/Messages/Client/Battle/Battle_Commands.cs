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
            ShowValues();
            Thread.Sleep((int)TimeSpan.FromSeconds(5).TotalSeconds);

            Avatar _Enemy = Resources.Battles.GetEnemy(this.Device.Player.Avatar.BattleID, this.Device.Player.Avatar.UserId);

            Resources.Battles[this.Device.Player.Avatar.BattleID].Tick = this.CommandTick;
            Resources.Battles[this.Device.Player.Avatar.BattleID].Checksum = this.CommandSum;
            if (this.CommandID == 1)
            {
                Resources.Battles[this.Device.Player.Avatar.BattleID].Commands.Enqueue(new Place_Troop(_Enemy.Device)
                {
                    SenderHigh = this.Device.Player.Avatar.UserHighId,
                    SenderLow =  this.Device.Player.Avatar.UserLowId
                });

                /* new Battle_Command_Data(this.Client)
                {
                    Sender      = this.Client.Level.PlayerID,
                    Tick        = this.CommandTick,
                    Checksum    = this.CommandSum
                }.Send();

                new Battle_Command_Data(_Enemy.Client)
                {
                    Sender      = this.Client.Level.PlayerID,
                    Tick        = this.CommandTick,
                    Checksum    = this.CommandSum
                }.Send(); */
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
                    CommandSenderHigh = this.Device.Player.Avatar.UserHighId,
                    CommandSenderLow = this.Device.Player.Avatar.UserLowId,
                }.Send();
            }
        }
    }
}
