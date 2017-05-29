using System;
using BL.Servers.CoC.Core;
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
            this.Device.Player.Avatar.Variables.Set(Variable.VillageToGoTo, this.Reader.ReadInt32());
            this.Tick = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
#if DEBUG       
            Loggers.Log($"Village Manager : Changing mode to {(Village_Mode)this.Device.Player.Avatar.Variables.Get(Variable.VillageToGoTo)}", true);
#endif
        }
    }
}
