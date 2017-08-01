using Magic.ClashOfClans;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Network.Messages.Client;
using Magic.Packets.Messages.Client;
using System;
using System.Collections.Generic;
using System.IO;

namespace Magic.ClashOfClans.Network
{
    internal static class MessageFactory
    {
        private static readonly Dictionary<int, Type> _messages;

        static MessageFactory()
        {
            _messages = new Dictionary<int, Type>();
            _messages.Add(10100, typeof(HandshakeRequest));
            _messages.Add(10101, typeof(LoginMessage));
            _messages.Add(10105, typeof(AskForFriendListMessage));
            _messages.Add(10108, typeof(KeepAliveMessage));
            _messages.Add(10117, typeof(ReportPlayerMessage));
            // m_vMessages.Add(10118, typeof(AccountSwitchMessage));
            _messages.Add(10113, typeof(GetDeviceTokenMessage));
            _messages.Add(10212, typeof(ChangeAvatarNameMessage));
            _messages.Add(10502, typeof(AddClashFriendMessage));
            _messages.Add(10905, typeof(NewsSeenMessage));
            _messages.Add(14100, typeof(AttackResultMessage));
            _messages.Add(14101, typeof(GoHomeMessage));
            _messages.Add(14102, typeof(ExecuteCommandsMessage));
            _messages.Add(14106, typeof(RevengeAttackerMessage));
            _messages.Add(14110, typeof(ChallengeWatchLiveMessage));
            _messages.Add(14111, typeof(ChallengeVisitMessage));
            _messages.Add(14113, typeof(VisitHomeMessage));
            _messages.Add(14114, typeof(ReplayRequestMessage));
            _messages.Add(14120, typeof(ChallengeAttackMessage));
            _messages.Add(14125, typeof(ChallengeCancelMessage));
            _messages.Add(14134, typeof(AttackNpcMessage));
            _messages.Add(14201, typeof(FacebookLinkMessage));
            _messages.Add(14316, typeof(EditClanSettingsMessage));
            _messages.Add(14301, typeof(CreateAllianceMessage));
            _messages.Add(14302, typeof(AskForAllianceDataMessage));
            _messages.Add(14303, typeof(AskForJoinableAlliancesListMessage));
            _messages.Add(14305, typeof(JoinAllianceMessage));
            _messages.Add(14306, typeof(PromoteAllianceMemberMessage));
            _messages.Add(14308, typeof(LeaveAllianceMessage));
            _messages.Add(14310, typeof(DonateAllianceUnitMessage));
            _messages.Add(14315, typeof(ChatToAllianceStreamMessage));
            _messages.Add(14317, typeof(JoinRequestAllianceMessage));
            _messages.Add(14321, typeof(TakeDecisionJoinRequestMessage));
            _messages.Add(14322, typeof(AllianceInviteMessage));
            _messages.Add(14324, typeof(SearchAlliancesMessage));
            _messages.Add(14325, typeof(AskForAvatarProfileMessage));
            _messages.Add(14331, typeof(AskForAllianceWarDataMessage));
            _messages.Add(14336, typeof(AskForAllianceWarHistoryMessage));
            _messages.Add(14341, typeof(AskForBookmarkMessage));
            _messages.Add(14343, typeof(AddToBookmarkMessage));
            _messages.Add(14344, typeof(RemoveFromBookmarkMessage));
            _messages.Add(14715, typeof(SendGlobalChatLineMessage));
            _messages.Add(14401, typeof(TopGlobalAlliancesMessage));
            _messages.Add(14402, typeof(TopLocalAlliancesMessage));
            _messages.Add(14403, typeof(TopGlobalPlayersMessage));
            _messages.Add(14404, typeof(TopLocalPlayersMessage));
            _messages.Add(14406, typeof(TopPreviousGlobalPlayersMessage));
            _messages.Add(14503, typeof(TopLeaguePlayersMessage));
            _messages.Add(14600, typeof(RequestAvatarNameChange));
            // m_vMessages.Add(15001, typeof(AllianceWarVisitMessage));
            _messages.Add(15001, typeof(AllianceWarAttackAvatarMessage));
        }

        public static Message Read(Client client, PacketReader reader, int messageId)
        {
            if (_messages.ContainsKey(messageId))
                return (Message)Activator.CreateInstance(_messages[messageId], client, reader);

            return null;
        }
    }
}
