using System;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;

namespace BL.Servers.CoC.Packets.Messages.Server
{
    internal class Own_Home_Data : Message
    {
        internal Level Avatar;

        internal Own_Home_Data(Device Device) : base(Device)
        {
            this.Identifier = 24101;
            this.Device.Player.Tick();
        }

        internal override void Encode()
        {
            using (Objects Home = new Objects(Avatar = this.Device.Player, Avatar.JSON))
            {
                this.Data.AddInt(0);
                this.Data.AddInt(0);

                this.Data.AddRange(Home.ToBytes);
                this.Data.AddRange(Avatar.Avatar.ToBytes);
                this.Data.AddInt(this.Device.State == State.WAR_EMODE ? 1 : 0);
                this.Data.AddInt(0);

                this.Data.AddLong(1462629754000);
                this.Data.AddLong(1462629754000);
                this.Data.AddLong(1462629754000);
            }
        }
    }
}
