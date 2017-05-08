using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;

namespace BL.Servers.CoC.Packets.Messages.Client
{
    internal class Execute_Commands : Message
    {
        internal int CTick;
        internal int STick;
        internal uint Checksum;
        internal int Count;

        internal byte[] Commands;
        internal List<Command> LCommands;


        public Execute_Commands(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override void Decode()
        {
            this.CTick = this.Reader.ReadInt32();
            this.Checksum = this.Reader.ReadUInt32();
            this.Count = this.Reader.ReadInt32();

            this.STick += (int)Math.Floor(DateTime.UtcNow.Subtract(this.Device.Player.Avatar.LastTick).TotalSeconds * 20);
            this.LCommands = new List<Command>((int)this.Count);
            this.Commands = this.Reader.ReadBytes((int)(this.Reader.BaseStream.Length - this.Reader.BaseStream.Position));
        }

        internal override void Process()
        {
            if (this.Count > -1 && this.Count <= 50)
            {
                this.Device.Player.Tick();
                using (Reader Reader = new Reader(this.Commands))
                {
                    for (int _Index = 0; _Index < this.Count; _Index++)
                    {
                        int CommandID = Reader.ReadInt32();
                        if (CommandFactory.Commands.ContainsKey(CommandID))
                        {
                            Command Command = Activator.CreateInstance(CommandFactory.Commands[CommandID], Reader, this.Device, CommandID) as Command;

                            if (Command != null)
                            {
#if DEBUG
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine("Command " + CommandID + " has  been handled.");
                                Console.ResetColor();
#endif
                                Command.Decode();
                                Command.Process();

                                this.LCommands.Add(Command);
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Command " + CommandID + " has not been handled.");
                            if (this.LCommands.Any()) Console.WriteLine("Previous command was " + this.LCommands.Last().Identifier + ". [" + (_Index + 1) + " / " + this.Count + "]");
                            Console.ResetColor();
                            break;
                        }
                    }
                }
            }
            else
            {
                //new OutOfSyncMessage(this.Device).Send();
            }
        }
    }
}
