using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL.Servers.CoC.Core;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Logic;

namespace BL.Servers.CoC.Packets.Debugs
{
    internal class Statistics : Debug
    {
        internal int Count;
        internal long UserID;
        internal StringBuilder Help;
        public Statistics(Device device, params string[] Parameters) : base(device, Parameters)
        {

        }

        internal override void Decode()
        {
            this.Count = this.Parameters.Length;
            switch (this.Count)
            {
                case 1:
                    if (!long.TryParse(this.Parameters[0], out this.UserID))
                    {
                        this.Help = new StringBuilder();
                        this.Help.AppendLine("Command:");
                        this.Help.AppendLine("\t/stats - Receive your own stats");
                        this.Help.AppendLine("\t/stats {player-tag} - Receive other user stats base on their tag");
                        SendChatMessage(Help.ToString());   
                    }
                    break;

                case 0:
                    this.UserID = this.Device.Player.Avatar.UserId;
                    break;
            }
        }

        internal override void Process()
        {
            if (this.UserID > 0)
            {
                var builder = new StringBuilder();
                var level = Resources.Players.Get(this.UserID, Constants.Database, false);
                if (level != null)
                {
                    builder.AppendLine($"Statistics for user {level.Avatar.Name}: ");
                    builder.AppendLine();
                    builder.AppendLine($"Play time: {level.Avatar.PlayTime.Hours}h {level.Avatar.PlayTime.Minutes}m {level.Avatar.PlayTime.Seconds}s");
                    builder.AppendLine($"Login count: {level.Avatar.Login_Count}");
                    builder.AppendLine($"Date joined: {level.Avatar.Created}");
                    builder.AppendLine($"Date saved: {level.Avatar.LastSave}");
                    SendChatMessage(builder.ToString());
                }
                else
                    SendChatMessage("Unable to fetch stats,Player is null");

            }
            else
            {
                this.Help = new StringBuilder();
                this.Help.AppendLine("Command:");
                this.Help.AppendLine("\t/stats - Receive your own stats");
                this.Help.AppendLine("\t/stats {player-tag} - Receive other user stats base on their tag");
                SendChatMessage(Help.ToString());
            }
        }

    }
}
