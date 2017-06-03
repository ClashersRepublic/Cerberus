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
            this.Tick = this.Reader.ReadVInt();
            this.Tick = this.Reader.ReadVInt();
            this.Reader.ReadInt16();

            this.Reader.ReadVInt();
            this.Reader.ReadVInt();
        }

        internal override void Process()
        {
            if (!Constants.BattlesDisabled)
            {
                if (Resources.Battles.Waiting.Count == 0)
                {
                    Resources.Battles.Enqueue(this.Device.Player);

                    this.Device.PlayerState = Logic.Enums.State.SEARCH_BATTLE;

                    new Matchmaking_Info(this.Device).Send();
                }
                else
                {
                    Player Enemy = Resources.Battles.Dequeue();

                    Battle Battle = new Battle(Enemy, this.Device.Player);

                    Resources.Battles.Add(Battle);

                    Battle.Player1.BattleID = Battle.BattleID;
                    Battle.Player2.BattleID = Battle.BattleID;

                    Battle.Player1.Device.PlayerState = Logic.Enums.State.IN_BATTLE;
                    Battle.Player2.Device.PlayerState = Logic.Enums.State.IN_BATTLE;

                    // Player 1
                    new Sector_PC(Battle.Player1.Device)
                    { 
                      Battle  = Battle,
                    }.Send();

                    new UDP_Connection_Info(Battle.Player1.Device).Send();

                    new Battle_End(Battle.Player1.Device).Send();

                    // Player 2
                    new Sector_PC(Battle.Player2.Device)
                    {
                        Battle = Battle,
                    }.Send();

                    new UDP_Connection_Info(Battle.Player2.Device).Send();

                    new Battle_End(Battle.Player2.Device).Send();
                }
            }
            else
            {
                new Matchmake_Failed(this.Device).Send();
                new Cancel_Battle_OK(this.Device).Send();
            }
        }
    }
}
