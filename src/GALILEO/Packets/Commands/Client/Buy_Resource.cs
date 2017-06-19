using System;
using Republic.Magic.Core.Networking;
using Republic.Magic.Extensions;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Logic;
using Republic.Magic.Logic.Enums;
using Republic.Magic.Packets.Messages.Server.Errors;

namespace Republic.Magic.Packets.Commands.Client
{
    internal class Buy_Resource : Command
    {
        internal int Resource_Count;
        internal int Resource_Data;
        internal int Gems_Price;
        internal bool EmbedCommands;

        public Buy_Resource(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.Resource_Count = this.Reader.ReadInt32();
            this.Resource_Data = this.Reader.ReadInt32();

            this.EmbedCommands = this.Reader.ReadBoolean();
            if (EmbedCommands)
            {
                this.Device.Depth++;
                if (this.Device.Depth >= MaxEmbeddedDepth)
                {
                    new Out_Of_Sync(this.Device).Send();
                    return;
                }
            }
            else
            {
                this.Device.Depth--;
            }
            this.Reader.ReadInt32(); 
        }

        internal override void Process()
        {
            this.Gems_Price = GameUtils.GetResourceDiamondCost(this.Resource_Count, this.Resource_Data);
            if (this.Device.Player.Avatar.Resources.Gems >= this.Gems_Price)
            {
                this.Device.Player.Avatar.Resources.Minus(Resource.Diamonds, this.Gems_Price);
                this.Device.Player.Avatar.Resources.Plus(this.Resource_Data, this.Resource_Count);
                if (EmbedCommands)
                {
                    int CommandID = Reader.ReadInt32();
                    if (CommandFactory.Commands.ContainsKey(CommandID))
                    {
                        Command Command =
                            Activator.CreateInstance(CommandFactory.Commands[CommandID], Reader, this.Device,
                                CommandID) as Command;

                        if (Command != null)
                        {
#if DEBUG
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("Command " + CommandID + " has  been handled.");
                            Console.ResetColor();
#endif
                            Command.Decode();
                            Command.Process();
                        }
                    }
                }   
            }
            else
            {
                new Out_Of_Sync(this.Device).Send();
            }
        }
    }
}
