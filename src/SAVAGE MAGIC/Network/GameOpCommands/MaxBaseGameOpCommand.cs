using System.IO;
using Magic.ClashOfClans.Core;

using Magic.Files.Logic;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network;
using Magic.ClashOfClans.Network.Messages.Server;
using Magic.Packets.Messages.Server;

namespace Magic.Packets.GameOpCommands
{
    internal class MaxBaseGameOpCommand : GameOpCommand
    {
        public static readonly string s_maxBase = File.ReadAllText("contents/max_home.json");

        public MaxBaseGameOpCommand(string[] Args)
        {
            RequiredPrivileges = 0;
        }

        public override void Execute(Level level)
        {
            level.SetHome(s_maxBase);
            new OwnHomeDataMessage(level.Client, level).Send();
        }
    }
}
