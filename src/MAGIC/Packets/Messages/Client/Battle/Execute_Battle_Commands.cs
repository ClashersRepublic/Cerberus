using System;
using System.Collections.Generic;
using System.Linq;
using CRepublic.Magic.Core;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Extensions;

namespace CRepublic.Magic.Packets.Messages.Client.Battle
{
    internal class Execute_Battle_Commands : Message
    {
        internal int CTick;
        internal int Checksum;
        internal int Count;

        internal byte[] Commands;
        internal List<Command> LCommands;


        public Execute_Battle_Commands(Device device) : base(device)
        {
        }

        internal override void Decode()
        {
            this.CTick = this.Reader.ReadInt32();
            this.Checksum = this.Reader.ReadInt32();
            this.Count = this.Reader.ReadInt32();

            this.LCommands = new List<Command>((int)this.Count);
            this.Commands = this.Reader.ReadBytes((int)(this.Reader.BaseStream.Length - this.Reader.BaseStream.Position));
        }

        internal override void Process()
        {
            if (this.Device.State == Logic.Enums.State.IN_1VS1_BATTLE)
            {
                Resources.Battles_V2.GetPlayer(this.Device.Player.Avatar.Battle_ID_V2, this.Device.Player.Avatar.UserId).Battle_Tick = CTick;
            }

            if (this.Count > -1 && Constants.MaxCommand == 0 || this.Count > -1 && this.Count <= Constants.MaxCommand)
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
                //new OutOfSyncMessage(this.Device).Send();
            }
        }
    }
}
