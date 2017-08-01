using System;
using System.Collections.Generic;
using System.IO;
using Savage.Magic.Core;

using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Logic.StreamEntries;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.Messages.Client
{
    internal class JoinRequestAllianceMessage : Message
    {
        public string Message;
        public long Id;

        public JoinRequestAllianceMessage(ClashOfClans.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
        }

        public override void Decode()
        {
            Id = Reader.ReadInt64();
            Message = Reader.ReadString();
        }


        public override void Process(Level level)
        {
            var avatar = level.Avatar;
            var alliance = ObjectManager.GetAlliance(Id);
            var streamEntry = new InvitationStreamEntry();
            streamEntry.SetId(alliance.ChatMessages.Count + 1);
            streamEntry.SetSenderId(avatar.Id);
            streamEntry.SetHomeId(avatar.Id);
            streamEntry.SetSenderLeagueId(avatar.GetLeagueId());
            streamEntry.SetSenderName(avatar.GetAvatarName());
            streamEntry.SetSenderRole(avatar.GetAllianceRole());
            streamEntry.SetMessage(Message);
            streamEntry.SetState(1);
            alliance.AddChatMessage(streamEntry);

            var members = alliance.AllianceMembers;
            foreach (var member in members)
            {
                var memberLevel = ResourcesManager.GetPlayer(member.GetAvatarId(), false);
                var memberClient = memberLevel.Client;
                if (memberClient != null)
                {
                    var message = new AllianceStreamEntryMessage(memberClient);
                    message.SetStreamEntry(streamEntry);
                    message.Send();
                }
            }
        }
    }
}