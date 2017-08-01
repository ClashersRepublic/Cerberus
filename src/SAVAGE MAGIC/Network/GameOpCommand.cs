using Magic.ClashOfClans;
using Magic.ClashOfClans.Core;

using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;
using System;

namespace Magic.ClashOfClans.Network
{
    internal class GameOpCommand
    {
        public static void SendCommandFailedMessage(Client c)
        {
            Console.WriteLine("GameOp command failed. Insufficient privileges. Requster ID -> " + c.Level.Avatar.Id);
            var p = new GlobalChatLineMessage(c);
            p.SetChatMessage("GameOp command failed. Insufficient privileges.");
            p.SetPlayerId(0);
            p.SetLeagueId(22);
            p.SetPlayerName("Clash of Magic");
            p.Send();
        }

        public virtual void Execute(Level level)
        {
            // Space
        }

        public byte RequiredPrivileges { get; set; }
    }
}