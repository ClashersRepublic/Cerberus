using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Files;
using BL.Servers.CoC.Files.CSV_Logic;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Packets.Messages.Server;
using Resource = BL.Servers.CoC.Logic.Enums.Resource;

namespace BL.Servers.CoC.Packets.Debugs
{
    internal class Resource_Update : Debug
    {
        internal int Count;
        internal int ResourceID;
        internal StringBuilder Help;

        public Resource_Update(Device device, params string[] Parameters) : base(device, Parameters)
        {

        }

        internal override void Decode()
        {
            this.Count = this.Parameters.Length;
            switch (this.Count)
            {
                case 1:
                    if (!int.TryParse(this.Parameters[0], out this.ResourceID))
                        this.ResourceID = -2;
                    break;

                default:
                    this.Help = new StringBuilder();
                    this.Help.AppendLine("Available resource types:");
                    this.Help.AppendLine("\t 0 = Diamonds");
                    this.Help.AppendLine("\t 1 = Gold");
                    this.Help.AppendLine("\t 2 = Elixir");
                    this.Help.AppendLine("\t 3 = DarkElixir");
                    this.Help.AppendLine("\t 7 = BuilderElixir");
                    this.Help.AppendLine("\t 8 = BuilderGold");
                    this.Help.AppendLine("\t 9 = All");
                    this.Help.AppendLine();
                    this.Help.AppendLine("Command:");
                    this.Help.AppendLine("\t/resource {resource-id}");
                    SendChatMessage(Help.ToString());
                    break;
            }
        }

        internal override void Process()
        {
            if (this.Count == 1)
            {
                switch (this.ResourceID)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 7:
                    case 8:
                        var resource = (Resource) this.ResourceID;
                        SendChatMessage($"Your resource amount for {resource} have been updated to 200000000");
                        this.Device.Player.Avatar.Resources.Set(resource, 200000000);
                        new Own_Home_Data(this.Device).Send();
                        break;

                    case 9:
                        this.Device.Player.Avatar.Resources.Set(Resource.Diamonds, 200000000);
                        this.Device.Player.Avatar.Resources.Set(Resource.Gold, 200000000);
                        this.Device.Player.Avatar.Resources.Set(Resource.Elixir, 200000000);
                        this.Device.Player.Avatar.Resources.Set(Resource.DarkElixir, 200000000);
                        this.Device.Player.Avatar.Resources.Set(Resource.Builder_Elixir, 200000000);
                        this.Device.Player.Avatar.Resources.Set(Resource.Builder_Gold, 200000000);

                        SendChatMessage($"All of your resource amount have been updated to 200000000");
                        new Own_Home_Data(this.Device).Send();
                        break;

                    default:
                        this.Help = new StringBuilder();
                        this.Help.AppendLine("Available resource types:");
                        this.Help.AppendLine("\t 0 = Diamonds");
                        this.Help.AppendLine("\t 1 = Gold");
                        this.Help.AppendLine("\t 2 = Elixir");
                        this.Help.AppendLine("\t 3 = DarkElixir");
                        this.Help.AppendLine("\t 7 = BuilderElixir");
                        this.Help.AppendLine("\t 8 = BuilderGold");
                        this.Help.AppendLine("\t 9 = All");
                        this.Help.AppendLine();
                        this.Help.AppendLine("Command:");
                        this.Help.AppendLine("\t/resource {resource-id}");
                        SendChatMessage(Help.ToString());
                        break;

                }
            }
        }
    }
}