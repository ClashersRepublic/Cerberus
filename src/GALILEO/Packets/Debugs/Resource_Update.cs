using System.Text;
using Republic.Magic.Core.Networking;
using Republic.Magic.Logic;
using Republic.Magic.Packets.Messages.Server;
using Resource = Republic.Magic.Logic.Enums.Resource;

namespace Republic.Magic.Packets.Debugs
{
    internal class Resource_Update : Debug
    {
        internal int ResourceID;
        internal StringBuilder Help;

        public Resource_Update(Device device, params string[] Parameters) : base(device, Parameters)
        {

        }

        internal override void Process()
        {
            if (this.Parameters.Length >= 1)
            {
                if (int.TryParse(this.Parameters[0], out this.ResourceID))
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
                else
                {
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
                }
            }
            else
            {
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
            }
        }
    }
}