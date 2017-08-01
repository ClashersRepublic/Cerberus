using Savage.Magic.Core;

using Savage.Magic.Logic;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.GameOpCommands
{
    internal class BecomeLeaderGameOpCommand : GameOpCommand
    {
        readonly string[] m_vArgs;

        public BecomeLeaderGameOpCommand(string[] args)
        {
            m_vArgs = args;
            RequiredPrivileges = 5;
        }

        public override void Execute(Level level)
        {
            if (level.AccountPrivileges>= RequiredPrivileges)
            {
                var clanid = level.Avatar.GetAllianceId();
                if (clanid != 0)
                {
                    foreach (
                        var pl in
                            ObjectManager.GetAlliance(level.Avatar.GetAllianceId()).AllianceMembers)
                        if (pl.GetRole() == 2)
                        {
                            pl.SetRole(4);
                            break;
                        }
                    level.Avatar.SetAllianceRole(2);
                }
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
    }
}
