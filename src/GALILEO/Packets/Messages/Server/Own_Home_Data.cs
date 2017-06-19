using System;
using Republic.Magic.Extensions.List;
using Republic.Magic.Logic;
using Republic.Magic.Logic.Enums;

namespace Republic.Magic.Packets.Messages.Server
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
                this.Data.AddInt((int)(Home.Timestamp - DateTime.UtcNow).TotalSeconds);
                this.Data.AddInt(-1);

                this.Data.AddRange(Home.ToBytes);
                this.Data.AddRange(Avatar.Avatar.ToBytes);
                this.Data.AddInt(this.Device.State == State.WAR_EMODE ? 1 : 0);
                this.Data.AddInt(0);

                this.Data.AddLong(1462629754000);
                this.Data.AddLong(1462629754000);
                this.Data.AddLong(1462629754000);
                this.Data.AddInt(0);
            }
        }
    }
}
