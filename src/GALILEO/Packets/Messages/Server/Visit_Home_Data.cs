using System;   
using Republic.Magic.Extensions.List;
using Republic.Magic.Logic;

namespace Republic.Magic.Packets.Messages.Server
{
    internal class Visit_Home_Data : Message
    {
        internal Level Player = null;
        internal Visit_Home_Data(Device Device, Level HomeOwner) : base(Device)
        {
            this.Identifier = 24113;
            this.Player = HomeOwner;
            this.Player.Tick();
            this.Device.State = Logic.Enums.State.VISIT;
        }

        internal override void Encode()
        {
            using (Objects Home = new Objects(Player, Player.JSON))
            {

                this.Data.AddInt((int)(Home.Timestamp - DateTime.UtcNow).TotalSeconds);

                this.Data.AddRange(Home.ToBytes);
                this.Data.AddRange(Player.Avatar.ToBytes);
                this.Data.AddInt(0);
                this.Data.Add(1);
                this.Data.AddRange(this.Device.Player.Avatar.ToBytes);
            }
        }
    }
}
