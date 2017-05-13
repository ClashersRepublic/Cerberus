using System;
using System.Collections.Generic;
using System.Timers;
using BL.Servers.CR.Core.Network;
using BL.Servers.CR.Packets;
using BL.Servers.CR.Packets.Messages.Server.Battle;

namespace BL.Servers.CR.Logic.Slots.Items
{
    internal class Battle
    {
        internal int BattleID;
        internal int Tick;
        internal int Checksum;

        internal Avatar Player1;
        internal Avatar Player2;

        internal Timer Timer = new Timer();
        internal Queue<Command> Commands = new Queue<Command>();

        public Battle()
        {
            
        }

        public Battle(Avatar _Player1, Avatar _Player2)
        {
            this.Player1 = _Player1;
            this.Player2 = _Player2;

            this.Timer = new Timer();
            this.Commands = new Queue<Command>();
        }

        public void Begin()
        {
            this.Timer.Interval = TimeSpan.FromSeconds(1).TotalMilliseconds;
            this.Timer.AutoReset = true;

            this.Timer.Elapsed += (Fuck, Gl) =>
            {
                new Battle_Command_Data(this.Player1.Device)
                {
                    Battle = this
                }.Send();

                new Battle_Command_Data(this.Player2.Device)
                {
                    Battle = this
                }.Send();
            };

            this.Timer.Start();

        }
        public void Stop()
        {
            this.Timer.Close();
        }
    }
}
