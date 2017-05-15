using BL.Servers.CoC.Core;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Logic.Structure.Slots.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CoC.Packets.Commands.Client
{
    internal class Request_Amical_Challenge : Command
    {
        internal string Message = string.Empty;

        public Request_Amical_Challenge(Reader _Reader, Device _Client, int _ID) : base(_Reader, _Client, _ID)
        {
        }
        internal override void Decode()
        {
            this.Message = this.Reader.ReadString();
            this.Reader.ReadByte();
            this.Reader.ReadByte();

            this.Reader.ReadInt32(); // Tick ?
        }
        internal override void Process()
        {
            var Clan = Resources.Clans.Get(this.Device.Player.Avatar.ClanId, Constants.Database);

            foreach (var Old_Entry in Clan.Chats.Slots.FindAll(M => M.Sender_ID == this.Device.Player.Avatar.UserId && M.Stream_Type == Alliance_Stream.AMICAL_BATTLE))
            {
                Clan.Chats.Remove(Old_Entry);
            }


            Clan.Chats.Add(
                new Entry
                {
                    Stream_Type = Alliance_Stream.AMICAL_BATTLE,

                    Sender_ID = this.Device.Player.Avatar.UserId,
                    Sender_Name = this.Device.Player.Avatar.Name,
                    Sender_Level = this.Device.Player.Avatar.Level,
                    Sender_League = this.Device.Player.Avatar.League,
                    Sender_Role = Clan.Members[this.Device.Player.Avatar.UserId].Role,
                    Message = this.Message
                });

        }
    }
}
