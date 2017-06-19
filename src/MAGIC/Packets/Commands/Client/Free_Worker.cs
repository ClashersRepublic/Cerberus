using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Packets.Messages.Server.Errors;

namespace CRepublic.Magic.Packets.Commands.Client
{
    internal class Free_Worker : Command
    {

        internal int Time_Left;
        internal int Unknown1;
        internal bool EmbedCommands;

        public Free_Worker(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.Time_Left = this.Reader.ReadInt32();
            this.Unknown1 = this.Reader.ReadInt32();
            this.EmbedCommands = this.Reader.ReadBoolean();

            if (this.EmbedCommands)
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
        }

        internal override void Process()
        {
            if (this.Device.Player.Avatar.Variables.IsBuilderVillage)
            {
                if (this.Device.Player.BuilderVillageWorkerManager.GetFreeWorkers() != 0)
                    return;
                this.Device.Player.BuilderVillageWorkerManager.FinishTaskOfOneWorker();
            }
            else
            {
                if (this.Device.Player.VillageWorkerManager.GetFreeWorkers() != 0)
                    return;
                this.Device.Player.VillageWorkerManager.FinishTaskOfOneWorker();
            }

            this.Device.Depth = 0;
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
    }
}
