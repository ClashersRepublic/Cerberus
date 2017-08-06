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
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Packets.Messages.Server.Errors;
using SharpRaven.Data;

namespace CRepublic.Magic.Packets.Messages.Client
{
    internal class Execute_Commands : Message
    {
        internal int CTick;
        internal uint Checksum;
        internal int Count;

        internal byte[] Commands;
        internal List<Command> LCommands;
        
        public Execute_Commands(Device device) : base(device)
        {
        }

        internal override void Decode()
        {
            Loggers.Log(this, Utils.Padding(this.Device.Player != null ? this.Device.Player.Avatar.UserId + ":" + GameUtils.GetHashtag(this.Device.Player.Avatar.UserId) : "Player is null", 15), Defcon.TRACE);
            this.CTick = this.Reader.ReadInt32();
            this.Checksum = this.Reader.ReadUInt32();
            this.Count = this.Reader.ReadInt32();

            this.Commands = this.Reader.ReadBytes((int)(this.Reader.BaseStream.Length - this.Reader.BaseStream.Position));

#if DEBUG
            this.LCommands = new List<Command>((int)this.Count);
#endif
        }

        internal override void Process()
        {
            if (!this.Device.Player.Avatar.Modes.IsAttackingOwnBase && this.Device.State == Logic.Enums.State.IN_PC_BATTLE)
                 Resources.Battles.Get(this.Device.Player.Avatar.Battle_ID).Battle_Tick = (int) this.CTick;
            
            if (this.Count > -1)
            {
                if (Constants.MaxCommand == 0 || this.Count <= Constants.MaxCommand)
                {
                        //this.Device.Player.Tick();
                        using (Reader Reader = new Reader(this.Commands))
                        {
                            for (int _Index = 0; _Index < this.Count; _Index++)
                            {
                                int CommandID = Reader.ReadInt32();
                                if (CommandFactory.Commands.ContainsKey(CommandID))
                                {
                                    var Command = Activator.CreateInstance(CommandFactory.Commands[CommandID], Reader,
                                        this.Device, CommandID) as Command;

                                    if (Command != null)
                                    {
#if DEBUG
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.WriteLine("Command " + CommandID + " has  been handled.");
                                    Console.ResetColor();
#endif
                                        try
                                        {
                                            Command.Decode();
                                        }
                                        catch (Exception Exception)
                                        {
                                            Exceptions.Log(Exception,
                                                Exception.Message + Environment.NewLine + Exception.StackTrace +
                                                Environment.NewLine + Exception.Data, this.Device.Model,
                                                this.Device.OSVersion,
                                                this.Device.Player.Avatar.Token,
                                                this.Device.Player?.Avatar.UserId ?? 0);

                                            Loggers.Log(Utils.Padding(Exception.GetType().Name, 15) + " : " +
                                                        Exception.Message + ". [" +
                                                        (this.Device.Player != null
                                                            ? this.Device.Player.Avatar.UserId + ":" +
                                                              GameUtils.GetHashtag(this.Device.Player.Avatar.UserId)
                                                            : "---") + ']' + Environment.NewLine + Exception.StackTrace,
                                                true,
                                                Defcon.ERROR);
                                        }

                                        try
                                        {
                                            Command.Process();
                                        }
                                        catch (Exception Exception)
                                        {
                                            Exceptions.Log(Exception,
                                                Exception.Message + Environment.NewLine + Exception.StackTrace +
                                                Environment.NewLine + Exception.Data, this.Device.Model,
                                                this.Device.OSVersion,
                                                this.Device.Player.Avatar.Token,
                                                this.Device.Player?.Avatar.UserId ?? 0);

                                            Loggers.Log(Utils.Padding(Exception.GetType().Name, 15) + " : " +
                                                        Exception.Message + ". [" +
                                                        (this.Device.Player != null
                                                            ? this.Device.Player.Avatar.UserId + ":" +
                                                              GameUtils.GetHashtag(this.Device.Player.Avatar.UserId)
                                                            : "---") + ']' + Environment.NewLine + Exception.StackTrace,
                                                true,
                                                Defcon.ERROR);
                                        }
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
                                if (this.LCommands.Any())
                                    Console.WriteLine("Previous command was " + this.LCommands.Last().Identifier +
                                                      ". [" + (_Index + 1) + " / " + this.Count + "]");
                                Console.ResetColor();
                                break;
#endif
                                }
                            }
                        }
                }
            }
            else
            {
                Exceptions.RavenClient.Capture(new SentryEvent($"Count value is weird {this.Count}"));
                new Out_Of_Sync(this.Device).Send();
            }
        }
    }
}
