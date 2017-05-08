namespace BL.Servers.BB.Packets.Messages.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BL.Servers.BB.Extensions.Binary;
    using BL.Servers.BB.Logic;

    internal class Execute_Commands : Message
    {
        internal uint CTick;
        internal int STick;
        internal ulong Checksum;
        internal uint Count;

        internal byte[] Commands;
        internal List<Command> LCommands;

        public Execute_Commands(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override void Decode()
        {
            this.CTick = this.Reader.ReadUInt32();
            this.Checksum = this.Reader.ReadUInt64();
            this.Reader.ReadByte();
            this.Count = this.Reader.ReadUInt32();

            this.LCommands = new List<Command>((int)this.Count);
            this.Commands = this.Reader.ReadBytes((int) (this.Reader.BaseStream.Length - this.Reader.BaseStream.Position));
        }

        internal override void Process()
        {
#if DEBUG
            this.Device.Player.Tick();
#endif
            if (this.Count > -1 && this.Count <= 50)
            {
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
        }
    }
}
