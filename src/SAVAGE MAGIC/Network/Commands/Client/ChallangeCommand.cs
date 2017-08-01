using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Savage.Magic.Core;
using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Logic.StreamEntries;
using Savage.Magic.Network;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.Commands
{
    internal class ChallangeCommand : Command
    {
        public string Message;

        public ChallangeCommand(PacketReader br)
        {
            Message = br.ReadString();
        }

        public override void Execute(Level level)
        {
            ChallangeCommand challangeCommand = this;

            Avatar player = level.Avatar;
            Alliance alliance = ObjectManager.GetAlliance(player.GetAllianceId());
            ChallengeStreamEntry cm = new ChallengeStreamEntry();
            cm.SetMessage(challangeCommand.Message);
            cm.SetSenderId(player.Id);
            cm.SetSenderName(player.GetAvatarName());
            cm.SetSenderLevel(player.GetAvatarLevel());
            ChallengeStreamEntry challengeStreamEntry = cm;
            int allianceRole = player.GetAllianceRole();
            challengeStreamEntry.SetSenderRole(allianceRole);
            challengeStreamEntry = (ChallengeStreamEntry)null;
            cm.SetId(alliance.ChatMessages.Count + 1);
            cm.SetSenderLeagueId(player.GetLeagueId());
            alliance.AddChatMessage((Savage.Magic.Logic.StreamEntries.StreamEntry)cm);
            Savage.Magic.Logic.StreamEntries.StreamEntry s = alliance.ChatMessages.Find((Predicate<Savage.Magic.Logic.StreamEntries.StreamEntry>)(c => c.GetStreamEntryType() == 12));
            List<AllianceMemberEntry>.Enumerator enumerator;
            if (s != null)
            {
                alliance.ChatMessages.RemoveAll((Predicate<Savage.Magic.Logic.StreamEntries.StreamEntry>)(t => t == s));
                foreach (AllianceMemberEntry allianceMember in alliance.AllianceMembers)
                {
                    Level player1 = ResourcesManager.GetPlayer(allianceMember.GetAvatarId(), false);
                    if (player1.Client != null)
                        new AllianceStreamEntryRemovedMessage(player1.Client, s.GetId()).Send();
                }
                enumerator = new List<AllianceMemberEntry>.Enumerator();
            }
            foreach (AllianceMemberEntry allianceMember in alliance.AllianceMembers)
            {
                Level player1 = ResourcesManager.GetPlayer(allianceMember.GetAvatarId(), false);
                if (player1.Client != null)
                {
                    AllianceStreamEntryMessage Message = new AllianceStreamEntryMessage(player1.Client);
                    ChallengeStreamEntry challengeStreamEntry1 = cm;
                    Message.SetStreamEntry((Savage.Magic.Logic.StreamEntries.StreamEntry)challengeStreamEntry1);
                    Message.Send();
                }
            }
            enumerator = new List<AllianceMemberEntry>.Enumerator();
            player = (Avatar)null;
            alliance = (Alliance)null;
            cm = (ChallengeStreamEntry)null;
        }
    }
}