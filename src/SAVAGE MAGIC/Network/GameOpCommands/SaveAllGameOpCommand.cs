using Magic.ClashOfClans.Core;

using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.GameOpCommands
{
    internal class SaveAllGameOpCommand : GameOpCommand
    {
        public SaveAllGameOpCommand(string[] args)
        {
            m_vArgs = args;
            RequiredPrivileges = 3;
        }

        public override void Execute(Level level)
        {
            if (level.AccountPrivileges>= RequiredPrivileges)
            {
                /* Starting saving of players */
                var pm = new GlobalChatLineMessage(level.Client);
                pm.SetChatMessage("Starting saving process of every player!");
                pm.SetPlayerId(0);
                pm.SetLeagueId(22);
                pm.SetPlayerName("UCS Bot");
                pm.Send();
                var levels = DatabaseManager.Instance.Save(ResourcesManager.GetInMemoryLevels());
                levels.Wait();
                var p = new GlobalChatLineMessage(level.Client);
                /* Confirmation */
                p.SetChatMessage("All Players are saved!");
                p.SetPlayerId(0);
                p.SetLeagueId(22);
                p.SetPlayerName("UCS Bot");
                p.Send();
                /* Starting saving of Clans */
                var pmm = new GlobalChatLineMessage(level.Client);
                pmm.SetPlayerId(0);
                pmm.SetLeagueId(22);
                pmm.SetPlayerName("UCS Bot");
                pmm.SetChatMessage("Starting with saving of every Clan!");
                pmm.Send();
                /* Confirmation */
                var clans = DatabaseManager.Instance.Save(ResourcesManager.GetInMemoryAlliances());
                clans.Wait();
                var pmp = new GlobalChatLineMessage(level.Client);
                pmp.SetPlayerId(0);
                pmp.SetLeagueId(22);
                pmp.SetPlayerName("UCS Bot");
                pmp.SetChatMessage("All Clans are saved!");
                pmp.Send();
            }
            else
            {
                var p = new GlobalChatLineMessage(level.Client);
                p.SetChatMessage("GameOp command failed. Access to Admin GameOP is prohibited.");
                p.SetPlayerId(0);
                p.SetLeagueId(22);
                p.SetPlayerName("UCS Bot");
                p.Send();
            }
        }
        readonly string[] m_vArgs;
    }
}
    