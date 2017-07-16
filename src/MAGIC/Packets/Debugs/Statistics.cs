using System.Text;
using CRepublic.Magic.Core;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Logic;

namespace CRepublic.Magic.Packets.Debugs
{
    internal class Statistics : Debug
    {
        internal long UserID;
        internal StringBuilder Help;
        public Statistics(Device device, params string[] Parameters) : base(device, Parameters)
        {

        }

        internal override void Process()
        {
            if (this.Parameters.Length >= 1)
            {
                if (long.TryParse(this.Parameters[0], out this.UserID))
                {
                    var builder = new StringBuilder();
                    var level = Resources.Players.Get(this.UserID, false);
                    if (level != null)
                    {
                        builder.AppendLine($"Statistics for user {level.Avatar.Name}: ");
                        builder.AppendLine();
                        builder.AppendLine(
                            $"Play time: {level.Avatar.PlayTime.Hours}h {level.Avatar.PlayTime.Minutes}m {level.Avatar.PlayTime.Seconds}s");
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
                    this.UserID = GameUtils.GetUserID(this.Parameters[0]);
                    if (this.UserID != -1)
                    {
                        var builder = new StringBuilder();
                        var level = Resources.Players.Get(this.UserID, false);
                        if (level != null)
                        {
                            builder.AppendLine($"Statistics for user {level.Avatar.Name}: ");
                            builder.AppendLine();
                            builder.AppendLine(
                                $"Play time: {level.Avatar.PlayTime.Hours}h {level.Avatar.PlayTime.Minutes}m {level.Avatar.PlayTime.Seconds}s");
                            builder.AppendLine($"Login count: {level.Avatar.Login_Count}");
                            builder.AppendLine($"Date joined: {level.Avatar.Created}");
                            builder.AppendLine($"Date saved: {level.Avatar.LastSave}");
                            SendChatMessage(builder.ToString());
                        }
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
            else
            {
                var builder = new StringBuilder();
                builder.AppendLine($"Statistics for user {this.Device.Player.Avatar.Name}: ");
                builder.AppendLine();
                builder.AppendLine($"Play time: {this.Device.Player.Avatar.PlayTime.Hours}h {this.Device.Player.Avatar.PlayTime.Minutes}m {this.Device.Player.Avatar.PlayTime.Seconds}s");
                builder.AppendLine($"Login count: {this.Device.Player.Avatar.Login_Count}");
                builder.AppendLine($"Date joined: {this.Device.Player.Avatar.Created}");
                builder.AppendLine($"Date saved: {this.Device.Player.Avatar.LastSave}");
                SendChatMessage(builder.ToString());
            }
        }
    }
}
