using CRepublic.Magic.Core;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Logic.Structure.Slots.Items;
using CRepublic.Magic.Packets.Commands.Server;
using CRepublic.Magic.Packets.Messages.Server;
using CRepublic.Magic.Packets.Messages.Server.Clans;

namespace CRepublic.Magic.Packets.Messages.Client.Clans
{
    internal class Take_Decision : Message
    {
        internal int Stream_High_ID;
        internal int Stream_Low_ID;
        internal byte Decision;

        public Take_Decision(Device device) : base(device)
        {
        }

        internal override void Decode()
        {
            this.Stream_High_ID = this.Reader.ReadInt32();
            this.Stream_Low_ID = this.Reader.ReadInt32();
            this.Decision = this.Reader.ReadByte();
        }

        internal override void Process()
        {
            var Alliance = Resources.Clans.Get(this.Device.Player.Avatar.ClanId, false);
            Entry Stream = Alliance.Chats.Get(this.Stream_Low_ID);
            if (Stream != null)
            {
                var Player = Players.Get(Stream.Sender_ID, false);
                if (Player.Avatar.ClanId == 0)
                {
                    if (Decision == 1)
                    {
                        if (Alliance.Members.Count < 50)
                        {
                            Player.Avatar.ClanId = Alliance.Clan_ID;
                            Player.Avatar.Alliance_Name = Alliance.Name;
                            Player.Avatar.Alliance_Level = Alliance.Level;
                            Player.Avatar.Alliance_Role = (int) Role.Member;
                            Player.Avatar.Badge_ID = Alliance.Badge;

                            Alliance.Members.Add(Player.Avatar);

                            if (Player.Device != null)
                            {
                                new Server_Commands(Player.Device)
                                {
                                    Command = new Joined_Alliance(Player.Device) { Clan = Alliance}
                                }.Send();

                                new Alliance_All_Stream_Entry(Player.Device).Send();
                            }

                            Alliance.Chats.Add(new Entry
                            {
                                Stream_Type = Alliance_Stream.EVENT,
                                Sender_ID = Player.Avatar.UserId,
                                Sender_Name = Player.Avatar.Name,
                                Sender_Level = Player.Avatar.Level,
                                Sender_League = Player.Avatar.League,
                                Sender_Role = Role.Member,
                                Event_ID = Events.ACCEPT_MEMBER,
                                Event_Player_ID = this.Device.Player.Avatar.UserId,
                                Event_Player_Name = this.Device.Player.Avatar.Name
                            });
                        }
                    }

                    Stream.Judge_Name = this.Device.Player.Avatar.Name;
                    Stream.Stream_State = Decision == 1 ? InviteState.ACCEPTED : InviteState.REFUSED;

                    foreach (Member Member in Alliance.Members.Values)
                    {
                        if (Member.Connected)
                        {
                            new Alliance_Stream_Entry(Member.Player.Device, Stream).Send();
                        }
                    }
                }
                else
                {
                    Alliance.Chats.Remove(Stream);
                }
            }

        }
    }
}
