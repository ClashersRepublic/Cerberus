namespace BL.Servers.CR.Packets.Messages.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BL.Servers.CR.Extensions.Binary;
    using BL.Servers.CR.Logic;

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
            this.CTick = (uint)this.Reader.ReadVInt();
            this.Checksum = (uint)this.Reader.ReadVInt();
            this.Count = (uint)this.Reader.ReadVInt();

            this.LCommands = new List<Command>((int)this.Count);
            this.Commands = this.Reader.ReadBytes((int) (this.Reader.BaseStream.Length - this.Reader.BaseStream.Position));
        }

        internal override void Process()
        {
            this.Device.Tick = (int) this.CTick;
            this.Device.LastChecksum = (int) this.Checksum;

            if (this.Count > -1 && this.Count <= 50)
            {
                using (Reader Reader = new Reader(this.Commands))
                {
                    for (int _Index = 0; _Index < this.Count; _Index++)
                    {
                        int CommandID = Reader.ReadVInt();
                        if (CommandFactory.Commands.ContainsKey(CommandID))
                        {
                            Command Command = Activator.CreateInstance(CommandFactory.Commands[CommandID], Reader, this.Device, CommandID) as Command;

                            if (Command != null)
                            {
                                System.Diagnostics.Debug.WriteLine("[COMMAND] " + CommandID + " has been handled.");

                                Command.Decode();
                                Command.Process();

                                this.LCommands.Add(Command);
                            }
                        }
                        else
                        {              
                            System.Diagnostics.Debug.WriteLine("[COMMAND] " + CommandID + " has not been handled.");
                            System.Diagnostics.Debug.WriteIf(this.LCommands.Any(), "[PREVIOUS] " + this.LCommands.Last().Identifier + " [" + (_Index + 1) + " / " + this.Count + "]");

                            break;
                        }
                    }
                }
            }
        }
    }
}
