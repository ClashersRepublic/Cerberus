using System;
using System.Linq;
using Republic.Magic.Core;
using Republic.Magic.Core.Networking;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Logic;
using Republic.Magic.Packets.Messages.Server;

namespace Republic.Magic.Packets.Messages.Client
{
    internal class Add_Global_Chat : Message 
    {
        internal string Message = string.Empty;
        
        public Add_Global_Chat(Device Device, Reader Reader) : base(Device, Reader)
        {
        }
        
        internal override void Decode()
        {
            this.Message = this.Reader.ReadString();
        }
        
        internal override void Process()
        {
            if (this.Message.StartsWith(DebugFactory.Delimiter))
            {
                var command = this.Message.Remove(0, 1);
                var commands = command.Split(' ');
                if (commands.Length > 0)
                {
                    var commandName = commands[0];
                    if (DebugFactory.Debugs.ContainsKey(commandName))
                    {
                        var args = commands.Skip(1).ToArray();
                        Debug Debug = Activator.CreateInstance(DebugFactory.Debugs[commandName], this.Device, args) as Debug;

                        if (Debug != null)
                        {
#if DEBUG
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine($"Debug Command {commandName} has  been handled.");
                            Console.ResetColor();
#endif

                            try
                            {
                                Debug.Process();
                            }
                            catch (Exception ex)
                            {
                                Resources.Exceptions.Catch(ex,
                                    $"Unable to process debug command with ID: {commandName}" + Environment.NewLine +
                                    ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine +
                                    ex.Data, this.Device.Model, this.Device.OSVersion, this.Device.Player.Avatar.Token,
                                    this.Device.Player?.Avatar.UserId ?? 0);
                            }
                        }

                    }
                    else
                    {
                        SendChatMessage($"Unknown command '{commandName}'. Type '/help' for more information.");
                    }
                }
            }
            else
            {
                foreach (var _Device in Resources.GChat.Get_Chat(this.Device).Values.ToList())
                {
                    new Global_Chat_Entry(_Device)
                    {
                        Message = this.Message,
                        Message_Sender = this.Device.Player.Avatar,
                        Regex = true,
                        Sender = this.Device == _Device
                    }.Send();
                }
            }
        }
    }
}
