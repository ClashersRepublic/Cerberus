using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic;

namespace BL.Servers.CoC.Packets.Messages.Server.Battle
{
    internal class Pc_Battle_Data : Message
    {
        internal Level Enemy = null;

        public Pc_Battle_Data(Device Device) : base(Device)
        {
            this.Identifier = 24107;
        }

        internal override void Encode()
        {
            this.Enemy.Tick();
            using (Objects Home = new Objects(Enemy, Enemy.JSON))
            {
                this.Data.AddInt((int)(Home.Timestamp - DateTime.UtcNow).TotalSeconds);
                this.Data.AddInt(-1);

                this.Data.AddRange(Home.ToBytes);
                this.Data.AddRange(Enemy.Avatar.ToBytes);
                this.Data.AddRange(this.Device.Player.Avatar.ToBytes);

                this.Data.AddInt(3); // 1 : Amical        2 : next button disabled       3 : PvP 
                this.Data.AddInt(0);
                this.Data.Add(0);
            }
        }

        internal override void Process()
        {
            this.Device.Player.Avatar.Battle = new Logic.Battle();
            this.Device.State = Logic.Enums.State.IN_PC_BATTLE;
        }

    }
}
