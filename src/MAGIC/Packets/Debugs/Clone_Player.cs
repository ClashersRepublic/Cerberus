using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Magic.Core;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Packets.Messages.Server;

namespace CRepublic.Magic.Packets.Debugs
{
    internal class Clone_Player : Debug
    {
        internal long UserID;
        internal StringBuilder Help;
        public Clone_Player(Device device, params string[] Parameters) : base(device, Parameters)
        {

        }

        internal override void Process()
        {
            if (this.Parameters.Length >= 1)
            {
                if (long.TryParse(this.Parameters[0], out this.UserID))
                {
                    var level = Resources.Players.Get(this.UserID, Constants.Database, false);
                    if (level != null)
                    {
                        this.Device.Player.Avatar.Heroes_Health = level.Avatar.Heroes_Health.Clone();
                        this.Device.Player.Avatar.Heroes_States = level.Avatar.Heroes_States.Clone();
                        this.Device.Player.Avatar.Heroes_Modes = level.Avatar.Heroes_Modes.Clone();
                        this.Device.Player.Avatar.Heroes_Upgrades = level.Avatar.Heroes_Upgrades.Clone();
                        this.Device.Player.Avatar.Unit_Upgrades = level.Avatar.Unit_Upgrades.Clone();
                        this.Device.Player.Avatar.Spell_Upgrades = level.Avatar.Spell_Upgrades.Clone();

                        this.Device.Player.Avatar.Tutorials = level.Avatar.Tutorials;
                        this.Device.Player.Avatar.TownHall_Level = level.Avatar.TownHall_Level;
                        this.Device.Player.Avatar.Castle_Level = level.Avatar.Castle_Level;
                        this.Device.Player.Avatar.Castle_Total = level.Avatar.Castle_Total;
                        this.Device.Player.Avatar.Castle_Total_SP = level.Avatar.Castle_Total_SP;

                        this.Device.Player.Reset();
                        this.Device.Player.JSON = level.JSON;

                        new Own_Home_Data(this.Device).Send();
                    }
                    else
                        SendChatMessage("Unable to clone base,Player is null");
                }
                else
                {
                    this.UserID = GameUtils.GetUserID(this.Parameters[0]);
                    if (this.UserID != -1)
                    {
                        var level = Resources.Players.Get(this.UserID, Constants.Database, false);
                        if (level != null)
                        {
                            this.Device.Player.Avatar.Heroes_Health = level.Avatar.Heroes_Health.Clone();
                            this.Device.Player.Avatar.Heroes_States = level.Avatar.Heroes_States.Clone();
                            this.Device.Player.Avatar.Heroes_Modes = level.Avatar.Heroes_Modes.Clone();
                            this.Device.Player.Avatar.Heroes_Upgrades = level.Avatar.Heroes_Upgrades.Clone();
                            this.Device.Player.Avatar.Unit_Upgrades = level.Avatar.Unit_Upgrades.Clone();
                            this.Device.Player.Avatar.Spell_Upgrades = level.Avatar.Spell_Upgrades.Clone();

                            this.Device.Player.Avatar.Tutorials = level.Avatar.Tutorials;
                            this.Device.Player.Avatar.TownHall_Level = level.Avatar.TownHall_Level;
                            this.Device.Player.Avatar.Castle_Level = level.Avatar.Castle_Level;
                            this.Device.Player.Avatar.Castle_Total = level.Avatar.Castle_Total;
                            this.Device.Player.Avatar.Castle_Total_SP = level.Avatar.Castle_Total_SP;

                            this.Device.Player.Reset();
                            this.Device.Player.JSON = level.JSON;

                            new Own_Home_Data(this.Device).Send();

                        }
                        else
                            SendChatMessage("Unable to clone base,Player is null");
                    }
                    else
                    {
                        this.Help = new StringBuilder();
                        this.Help.AppendLine("Hashtags should only contain these characters:");
                        this.Help.AppendLine("Numbers: 0, 2, 8, 9");
                        this.Help.AppendLine("Letters: P, Y, L, Q, G, R, J, C, U, V");
                        SendChatMessage(Help.ToString());
                    }
                }
            }
        }
    }
}
