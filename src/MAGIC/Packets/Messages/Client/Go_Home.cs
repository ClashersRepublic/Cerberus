using System;
using CRepublic.Magic.Core;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Structure.Slots.Items;
using CRepublic.Magic.Packets.Messages.Server;
using CRepublic.Magic.Packets.Messages.Server.Battle;

namespace CRepublic.Magic.Packets.Messages.Client
{
    internal class Go_Home : Message
    {
        internal int State;

        public Go_Home(Device device) : base(device)
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
                        var Battle = Resources.Battles.Get(this.Device.Player.Avatar.Battle_ID);
                        if (Battle != null)
                        {
                            if (Battle.Commands.Count > 0)
                            {
                                Level Player = Core.Players.Get(Battle.Defender.UserId, false);

                                if (Utils.IsOdd(Resources.Random.Next(1, 1000)))
                                {
                                    int lost = (int) Battle.LoseTrophies();
                                    Player.Avatar.Trophies += (int) Battle.WinTrophies();

                                    if (this.Device.Player.Avatar.Trophies >= lost)
                                        this.Device.Player.Avatar.Trophies -= (int) Battle.LoseTrophies();
                                    else
                                        this.Device.Player.Avatar.Trophies = 0;

                                    Battle.Replay_Info.Stats.Defender_Score = (int) Battle.WinTrophies();
                                    Battle.Replay_Info.Stats.Attacker_Score = lost > 0 ? -lost : 0;
                                    Battle.Replay_Info.Stats.Destruction_Percentage = Resources.Random.Next(49);
                                }
                                else
                                {

                                    int lost = (int) Battle.LoseTrophies();
                                    if (Player.Avatar.Trophies >= lost)
                                        Player.Avatar.Trophies -= (int) Battle.LoseTrophies();
                                    else
                                        Player.Avatar.Trophies = 0;

                                    this.Device.Player.Avatar.Trophies += (int) Battle.WinTrophies();
                                    Battle.Replay_Info.Stats.Attacker_Score = (int) Battle.WinTrophies();
                                    Battle.Replay_Info.Stats.Defender_Score = lost > 0 ? -lost : 0;

                                    Battle.Replay_Info.Stats.Destruction_Percentage = Resources.Random.Next(50, 100);
                                    if (Battle.Replay_Info.Stats.Destruction_Percentage == 100)
                                        Battle.Replay_Info.Stats.TownHall_Destroyed = true;
                                }
                                this.Device.Player.Avatar.Refresh();
                                Player.Avatar.Refresh();

                                Battle.Set_Replay_Info();
                                this.Device.Player.Avatar.Inbox.Add(
                                    new Mail
                                    {
                                        Stream_Type = Logic.Enums.Avatar_Stream.ATTACK,
                                        Battle_ID = this.Device.Player.Avatar.Battle_ID
                                    });

                                //if (Core.Players.Get(Battle.Defender.UserId, Constants.Database) == null)
                                {
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
                                Core.Resources.Battles.TryRemove(this.Device.Player.Avatar.Battle_ID);
                        }
                        this.Device.Player.Avatar.Battle_ID = 0;
                    }

                    this.Device.State = Logic.Enums.State.LOGGED;
                }
                else if (this.Device.State == Logic.Enums.State.IN_AMICAL_BATTLE)
                {
                    var Alliance = Resources.Clans.Get(this.Device.Player.Avatar.ClanId, false);
                    Entry Stream = Alliance.Chats.Get(this.Device.Player.Avatar.Amical_ID);
                    if (Stream != null)
                    {
                        Alliance.Chats.Remove(Stream);
                    }
                    this.Device.State = Logic.Enums.State.LOGGED;
                }
                else if (this.Device.State == Logic.Enums.State.SEARCH_BATTLE)
                {
                    if (this.Device.Player.Avatar.Variables.IsBuilderVillage)
                    {
                        this.Device.Player.Avatar.Battle_ID_V2 = 0;

                        Resources.Battles_V2.Dequeue(this.Device.Player);
                    }

                    this.Device.State = Logic.Enums.State.LOGGED;
                }
                else if (this.Device.State == Logic.Enums.State.IN_1VS1_BATTLE)
                {

                    long UserID = this.Device.Player.Avatar.UserId;
                    long BattleID = this.Device.Player.Avatar.Battle_ID_V2;
                    var Home = Resources.Battles_V2.GetPlayer(BattleID, UserID);
                    var Enemy = Resources.Battles_V2.GetEnemy(BattleID, UserID);
                    var Battle = Resources.Battles_V2[BattleID];

                    Home.Set_Replay_Info();
                    Home.Finished = true;

                    if (Utils.IsOdd(Resources.Random.Next(1, 1000)))
                    {
                        Home.Replay_Info.Stats.Destruction_Percentage = Resources.Random.Next(49);
                    }
                    else
                    {
                        Home.Replay_Info.Stats.Destruction_Percentage = Resources.Random.Next(50, 100);
                        Home.Replay_Info.Stats.Attacker_Stars += 1;
                        if (Home.Replay_Info.Stats.Destruction_Percentage == 100)
                        {
                            Home.Replay_Info.Stats.TownHall_Destroyed = true;
                            Home.Replay_Info.Stats.Attacker_Stars += 2;
                        }
                    }

                    if (Enemy.Finished)
                    {
                        new V2_Battle_Result(Battle.GetEnemy(UserID).Device, Battle).Send();
                    }

                    new V2_Battle_Result(this.Device, Battle).Send();


                    this.Device.Player.Avatar.Battle_ID_V2 = 0;
                    this.Device.State = Logic.Enums.State.LOGGED;
                }

                new Own_Home_Data(this.Device).Send();

                if (this.Device.Player.Avatar.ClanId > 0)
                {
                    
                }
            }
        }
    }
}