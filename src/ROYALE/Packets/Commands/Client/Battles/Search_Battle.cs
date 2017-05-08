using System;
using BL.Servers.CR.Core;
using BL.Servers.CR.Core.Network;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;
using BL.Servers.CR.Logic.Slots.Items;
using BL.Servers.CR.Packets.Messages.Server;
using BL.Servers.CR.Packets.Messages.Server.Battle;

namespace BL.Servers.CR.Packets.Commands.Client.Battles
{
    internal class Search_Battle : Command
    {
        internal int Tick;

        public Search_Battle(Reader Reader, Device Device, int ID) : base(Reader, Device, ID)
        {

        }

        internal override void Decode()
        {
            this.Tick = this.Reader.ReadRRInt32();
            this.Tick = this.Reader.ReadRRInt32();
            this.Reader.ReadInt16();

            this.Reader.ReadRRInt32();
            this.Reader.ReadRRInt32();
        }

        internal override void Process()
        {
            if (!Constants.BattlesDisabled)
            {
                if (Resources.Battles.Waiting.Count == 0)
                {
                    Resources.Battles.Enqueue(this.Device.Player.Avatar);

                    Console.WriteLine($"Added {this.Device.Player.Avatar.UserId} into the queue!");

                    new Matchmaking_Info(this.Device).Send();                    // Player 1
                }
                else
                {
                    Avatar Enemy = Resources.Battles.Dequeue();
                    Battle Battle = new Battle(Enemy, this.Device.Player.Avatar);

                    Resources.Battles.Add(Battle);

                    Battle.Player1.BattleID = Battle.BattleID;
                    Battle.Player2.BattleID = Battle.BattleID;

                    Console.WriteLine($"Player 1: {Battle.Player1.UserId}");
                    Console.WriteLine($"Player 2: {Battle.Player2.UserId}");

                    // Player 1
                    new Sector_PC(Battle.Player1.Device)
                    { 
                      Battle  = Battle,
                    }.Send();
                   // new UDP_Connection_Info(Battle.Player1.Device).Send();

                    // Player 2
                    new Sector_PC(Battle.Player2.Device)
                    {
                        Battle = Battle,
                    }.Send();
                  //  new UDP_Connection_Info(Battle.Player2.Device).Send();
                }
            }
            else
            {
                new Matchmake_Failed(Device).Send();
                new Cancel_Battle_OK(Device).Send();
            }
        }
    }
}
