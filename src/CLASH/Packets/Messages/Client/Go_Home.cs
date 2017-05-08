using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Core.Database;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Packets.Messages.Server;

namespace BL.Servers.CoC.Packets.Messages.Client
{
    internal class Go_Home : Message
    {
        internal int State;

        public Go_Home(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Go_Home.
        }

        internal override void Decode()
        {
            this.State = this.Reader.ReadInt32();   
            this.Reader.ReadByte();     
        }
        internal override void Process()
        {
            this.Device.State = State == 1 ? Logic.Enums.State.WAR_EMODE : Logic.Enums.State.LOGGED;

            new Own_Home_Data(this.Device).Send();

            if (this.Device.Player.Avatar.ClanId > 0)
            {
            }
        }
    }
}
