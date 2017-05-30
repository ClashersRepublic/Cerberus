using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Core;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Logic.Structure.Slots.Items;
using BL.Servers.CoC.Packets.Messages.Server;
using BL.Servers.CoC.Packets.Messages.Server.Battle;
using BL.Servers.CoC.Packets.Messages.Server.Clans;

namespace BL.Servers.CoC.Packets.Messages.Client.Battle
{
    internal class Attack_Alliance_Challange : Message
    {
        internal int Stream_High_ID;
        internal int Stream_Low_ID;
        public Attack_Alliance_Challange(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Attack_Alliance_Challange.
        }

        internal override void Decode()
        {
            this.Stream_High_ID = this.Reader.ReadInt32();
            this.Stream_Low_ID = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            var Alliance = Resources.Clans.Get(this.Device.Player.Avatar.ClanId, Constants.Database, false);
            Entry Stream = Alliance.Chats.Get(this.Stream_Low_ID);
            if (Stream != null)
            {
                var Player = Resources.Players.Get(Stream.Sender_ID, Constants.Database, false);
                if (Player != null)
                {
                    this.Device.State = State.IN_AMICAL_BATTLE;
                    this.Device.Player.Avatar.Amical_ID = this.Stream_Low_ID;

                    new Pc_Battle_Data(this.Device){Enemy = Player, BattleMode = Battle_Mode.AMICAL}.Send();

                    Stream.Amical_State = Amical_Mode.LIVE_REPLAY;

                    foreach (Member Member in Alliance.Members.Values)
                    {
                        if (Member.Connected)
                        {
                            new Alliance_Stream_Entry(Member.Player.Client, Stream).Send();
                        }
                    }
                }
                else
                {
                    new Own_Home_Data(this.Device).Send();
                }
            }

        }
    }
}
