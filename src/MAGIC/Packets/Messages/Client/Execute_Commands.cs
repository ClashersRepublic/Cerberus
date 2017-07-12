using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CRepublic.Magic.Core;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Packets.Messages.Server.Errors;
using SharpRaven.Data;

namespace CRepublic.Magic.Packets.Messages.Client
{
    internal class Execute_Commands : Message
    {
        internal int CTick;
        internal int STick;
        internal int Checksum;
        internal int Count;

        internal byte[] Commands;
        internal List<Command> LCommands;
        
        public Execute_Commands(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override void Decode()
        {
            this.CTick = this.Reader.ReadInt32();
            this.Checksum = this.Reader.ReadInt32();
            this.Count = this.Reader.ReadInt32();

            this.STick += (int)Math.Floor(DateTime.UtcNow.Subtract(this.Device.Player.Avatar.LastTick).TotalSeconds * 20);
#if DEBUG
            this.LCommands = new List<Command>((int)this.Count);
#endif
            this.Commands = this.Reader.ReadBytes((int)(this.Reader.BaseStream.Length - this.Reader.BaseStream.Position));
        }

        internal override void Process()
        {

            if (this.Device.State == Logic.Enums.State.IN_PC_BATTLE)
                 Resources.Battles.Get(this.Device.Player.Avatar.Battle_ID, Constants.Database).Battle_Tick =
                    (int) this.CTick;
        
            if (this.Count > -1 && Constants.MaxCommand > 0 && this.Count <= Constants.MaxCommand)
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
#if DEBUG
                                this.LCommands.Add(Command);
#endif
                            }
                        }
                        else
                        {
#if DEBUG
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Command " + CommandID + " has not been handled.");
                            if (this.LCommands.Any()) Console.WriteLine("Previous command was " + this.LCommands.Last().Identifier + ". [" + (_Index + 1) + " / " + this.Count + "]");
                            Console.ResetColor();
                            break;
#endif
                        }
                    }
                }
            }
            else
            {
                Resources.Exceptions.RavenClient.Capture(new SentryEvent($"Count value is weird {this.Count}"));
                new Out_Of_Sync(this.Device).Send();
            }
        }
    }
}
