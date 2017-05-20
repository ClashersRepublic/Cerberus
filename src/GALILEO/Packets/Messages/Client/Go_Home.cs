using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Core;
using BL.Servers.CoC.Core.Database;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Structure.Slots.Items;
using BL.Servers.CoC.Packets.Messages.Server;

namespace BL.Servers.CoC.Packets.Messages.Client
{
    internal class Go_Home : Message
    {
        internal int State;

        public Go_Home(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Go_Home.
        }

        internal override void Decode()
        {
            this.State = this.Reader.ReadInt32();
            this.Reader.ReadByte();
        }

        internal override void Process()
        {

            if (State == 1)
            {
                this.Device.State = Logic.Enums.State.WAR_EMODE;
            }
            else
            {
                if (this.Device.State == Logic.Enums.State.IN_PC_BATTLE)
                {
                    if (this.Device.Player.Avatar.Battle_ID > 0)
                    {
                        var Battle =
                            Core.Resources.Battles.Get(this.Device.Player.Avatar.Battle_ID, Constants.Database);
                        if (Battle.Commands.Count > 0)
                        {
                            Battle.Set_Replay_Info();
                            this.Device.Player.Avatar.Inbox.Add(
                                new Mail
                                {
                                    Stream_Type = Logic.Enums.Avatar_Stream.ATTACK,
                                    Battle_ID = this.Device.Player.Avatar.Battle_ID
                                });

                            //if (Core.Resources.Players.Get(Battle.Defender.UserId, Constants.Database) == null)
                            {
                                Level Player =
                                    Core.Resources.Players.Get(Battle.Defender.UserId, Constants.Database, false);

                                //if (Player.Avatar.Guard < 1)
                                Player.Avatar.Inbox.Add(
                                    new Mail
                                    {
                                        Stream_Type = Logic.Enums.Avatar_Stream.DEFENSE,
                                        Battle_ID = this.Device.Player.Avatar.Battle_ID
                                    });
                            }
                            Core.Resources.Battles.Save(Battle);
                        }
                        else
                            Core.Resources.Battles.Remove(this.Device.Player.Avatar.Battle_ID);
                        this.Device.Player.Avatar.Battle_ID = 0;
                        this.Device.State = Logic.Enums.State.LOGGED;
                    }
                }
                else if (this.Device.State == Logic.Enums.State.IN_AMICAL_BATTLE)
                {
                    var Alliance = Resources.Clans.Get(this.Device.Player.Avatar.ClanId, Constants.Database, false);
                    Entry Stream = Alliance.Chats.Get(this.Device.Player.Avatar.Amical_ID);
                    if (Stream != null)
                    {
                        Alliance.Chats.Remove(Stream);
                    }
                }

                new Own_Home_Data(this.Device).Send();

                if (this.Device.Player.Avatar.ClanId > 0)
                {
                }
            }
        }
    }
}