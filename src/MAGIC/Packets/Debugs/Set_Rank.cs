using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Magic.Core;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;

namespace CRepublic.Magic.Packets.Debugs
{
    internal class Set_Rank : Debug
    {
        internal long UserID;
        internal Rank Rank;
        internal StringBuilder Help;
        public Set_Rank(Device device, params string[] Parameters) : base(device, Parameters)
        {

        }

        internal override void Process()
        {
            if (this.Device.Player.Avatar.Rank >= Rank.ADMIN)
            {
                if (this.Parameters.Length >= 2)
                {
                    try
                    {
                        if (Enum.TryParse(this.Parameters[1], out this.Rank))
                        {
                            if (long.TryParse(this.Parameters[0], out this.UserID))
                            {
                                var target = Resources.Players.Get(this.UserID);
                                if (this.Rank > this.Device.Player.Avatar.Rank)
                                {
                                    SendChatMessage(
                                        "Command Processor: Target privileges is higher then yours.");
                                }
                                else
                                {
                                    if (target != null)
                                    {
                                        target.Avatar.Rank = this.Rank;
                                        SendChatMessage($"Command Processor: Target is now become {this.Rank}");
                                    }
                                    else
                                        SendChatMessage(
                                            "Command Processor: Target is null.");
                                }
                            }
                            else
                            {
                                this.UserID = GameUtils.GetUserID(this.Parameters[0]);
                                if (this.UserID != -1)
                                {
                                    var target = Resources.Players.Get(this.UserID);
                                    if (this.Rank > this.Device.Player.Avatar.Rank)
                                    {
                                        SendChatMessage(
                                            "Command Processor: Target privileges is higher then yours.");
                                    }
                                    else
                                    {
                                        if (target != null)
                                        {
                                            target.Avatar.Rank = this.Rank;
                                            SendChatMessage($"Command Processor: Target is now become {this.Rank}");
                                        }
                                        else
                                            SendChatMessage("Command Processor: Target is null.");
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
                            this.Help = new StringBuilder();
                            this.Help.AppendLine("Available rank:");
                            this.Help.AppendLine("\t 1 = Normal Player");
                            this.Help.AppendLine("\t 2 = Donator");
                            this.Help.AppendLine("\t 3 = VIP");
                            this.Help.AppendLine("\t 4 = Moderator");
                            this.Help.AppendLine("\t 5 = Administrator");
                            if (this.Device.Player.Avatar.Rank == Rank.FOUNDER)
                                this.Help.AppendLine("\t 6 = Founder");

                            this.Help.AppendLine();
                            this.Help.AppendLine("Command:");
                            this.Help.AppendLine("\t/rank {user-id} {rank}");
                            this.Help.AppendLine("\t/rank {user-hashtag} {rank}");
                            SendChatMessage(this.Help.ToString());
                        }
                    }                 
                    catch (Exception ex)
                    {
                        SendChatMessage($"Command Processor: Failed with error {ex}");
                    }
                }
                else
                {
                    this.Help = new StringBuilder();
                    this.Help.AppendLine("Available rank:");
                    this.Help.AppendLine("\t 1 = Normal Player");
                    this.Help.AppendLine("\t 2 = Donator");
                    this.Help.AppendLine("\t 3 = VIP");
                    this.Help.AppendLine("\t 4 = Moderator");
                    this.Help.AppendLine("\t 5 = Administrator");
                    if (this.Device.Player.Avatar.Rank == Rank.FOUNDER)
                        this.Help.AppendLine("\t 6 = Founder");

                    this.Help.AppendLine();
                    this.Help.AppendLine("Command:");
                    this.Help.AppendLine("\t/rank {user-id} {rank}");
                    this.Help.AppendLine("\t/rank {user-hashtag} {rank}");
                    SendChatMessage(this.Help.ToString());
                }
            }
            else
            {
                SendChatMessage("Command Processor: Insufficient privileges.");
            }
        }
    }
}
