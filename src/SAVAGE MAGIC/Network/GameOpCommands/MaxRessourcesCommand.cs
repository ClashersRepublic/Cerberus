using Magic.ClashOfClans.Core;

using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.GameOpCommands
{
    internal class MaxRessourcesCommand : GameOpCommand
    {
        public MaxRessourcesCommand(string[] Args)
        {
            RequiredPrivileges = 0;
        }

        public override void Execute(Level level)
        {
            if (level.AccountPrivileges>= RequiredPrivileges)
            {
                var p = level.Avatar;
                p.SetResourceCount(CsvManager.DataTables.GetResourceByName("Gold"), 999999999);
                p.SetResourceCount(CsvManager.DataTables.GetResourceByName("Elixir"), 999999999);
                p.SetResourceCount(CsvManager.DataTables.GetResourceByName("DarkElixir"), 999999999);
                p.SetDiamonds(999999999);
                new OwnHomeDataMessage(level.Client, level).Send();
            }
            else
                SendCommandFailedMessage(level.Client);
        }
    }
}
