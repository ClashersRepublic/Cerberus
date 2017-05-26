using System;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;

namespace BL.Servers.CoC.Packets.Commands.Client
{
    internal class Change_Village_Mode : Command
    {
        internal int Tick;
        public Change_Village_Mode(Reader reader, Device client, int id) : base(reader, client, id)
        {
            
        }

        internal override void Decode()
        {
            this.Device.Player.Avatar.Village_Mode = (Village_Mode) this.Reader.ReadInt32();
            this.Tick = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            Console.WriteLine($"Village Manager : Changing mode to {this.Device.Player.Avatar.Village_Mode}");
        }
    }
}
