using System;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Packets.Messages.Server.Errors;

namespace BL.Servers.CoC.Packets.Commands.Client
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
                this.Device.Depth += 1;
                if (this.Device.Depth >= MaxEmbeddedDepth)
                {
                    new Out_Of_Sync(this.Device).Send();
                    Core.Resources.Gateway.Disconnect(this.Device.Token.Args);
                    return;
                }
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
            Console.WriteLine("Buy {0}   Resource : {1}", this.Resource_Count, this.Resource_Data);
        }
    }
}
