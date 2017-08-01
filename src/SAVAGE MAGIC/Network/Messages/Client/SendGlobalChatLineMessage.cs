using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Magic.ClashOfClans.Core;

using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    // Packet 14715
    internal class SendGlobalChatLineMessage : Message
    {
        static SendGlobalChatLineMessage()
        {
            s_bannedWords = File.ReadAllLines("./filter.ucs");

            // Avoid case sensitivity.
            for (int i = 0; i < s_bannedWords.Length; i++)
                s_bannedWords[i] = s_bannedWords[i].ToLower();
        }

        private static readonly string[] s_bannedWords;

        public SendGlobalChatLineMessage(ClashOfClans.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
        }

        public string Message { get; set; }

        public override void Decode()
        {
            Message = Reader.ReadString();
        }

        public override void Process(Level level)
        {
            if (Message.Length > 0)
            {
                if (Message[0] == '/')
                {
                    var cmd = (GameOpCommand)GameOpCommandFactory.Parse(Message);
                    cmd?.Execute(level);
                }
                else
                {
                    var senderId = level.Avatar.Id;
                    var senderName = level.Avatar.GetAvatarName();

                    if (File.Exists(@"filter.ucs"))
                    {
                        var messageLower = Message.ToLower();
                        var flagged = false;
                        for (int i = 0; i < s_bannedWords.Length; i++)
                        {
                            if (messageLower.Contains(s_bannedWords[i]))
                            {
                                flagged = true;
                                break;
                            }
                        }

                        if (flagged)
                        {
                            var message = new GlobalChatLineMessage(level.Client);
                            message.SetPlayerId(0);
                            message.SetPlayerName("Chat Filter System");
                            message.SetLeagueId(22);
                            message.SetChatMessage("We've detected banned words in your chat message.");
                            message.Send();
                            return;
                        }
                    }

                    var onlinePlayers = ResourcesManager.OnlinePlayers;
                    for (int i = 0; i < onlinePlayers.Count; i++)
                    {
                        var player = onlinePlayers[i];
                        var message = new GlobalChatLineMessage(player.Client);

                        if (player.AccountPrivileges > 0)
                            message.SetPlayerName(senderName + " #" + senderId);
                        else
                            message.SetPlayerName(senderName);

                        message.SetChatMessage(Message);
                        message.SetPlayerId(senderId);
                        message.SetLeagueId(level.Avatar.GetLeagueId());
                        message.SetAlliance(ObjectManager.GetAlliance(level.Avatar.GetAllianceId()));
                        message.Send();
                    }
                }
            }
        }
    }
}
