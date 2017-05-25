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
                this.Data.AddInt((int)(Home.Timestamp - DateTime.UtcNow).TotalSeconds);
                this.Data.AddInt(-1);

                this.Data.AddRange(Home.ToBytes);
                this.Data.AddHexa("00000042019165100000004201916510000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000ffffffff00000000000000000000000000000000000000000000000000000005537567636fffffffff0000000100000000");
                this.Data.AddInt(999999999);
                this.Data.AddInt(999999999);
                this.Data.AddHexa("000004b00000003c0000000000000000000000000000000000000000000000000000000000000000000000000000000001000000dc6cf5eb4800ffffffff000000000000000000000001000000000000000000");
                this.Data.AddInt(8);
                for (int i = 0; i < 8; i++)
                {
                    this.Data.AddInt(3000001 + i);
                    this.Data.AddInt(999999999);
                }
                this.Data.AddInt(8);
                for (int i = 0; i < 8; i++)
                {
                    this.Data.AddInt(3000001 + i);
                    this.Data.AddInt(999999999);
                }
                this.Data.AddDataSlots(Avatar.Avatar.Units);
                this.Data.AddDataSlots(Avatar.Avatar.Spells);
                this.Data.AddInt(0);
                this.Data.AddInt(0);
                this.Data.AddInt(0);
                this.Data.AddInt(0);
                this.Data.AddInt(0);
                this.Data.AddInt(0);
                this.Data.AddInt(24);
                for (int i = 0; i < 24; i++)
                {
                    this.Data.AddInt(21000000 + i);
                }
                this.Data.AddHexa("0000000000000003015ef3c600000001015ef3c700000001015ef3c8000000010000000000000000000000000000000000000000000000000000000202349342000000000234934e000000000000000000000000000000000000000000000000000000000000000000000000000000000000015c4e0223700000015c4e0223700000015c4e1d9ab000000000");
                /*
                this.Data.AddRange(Home.ToBytes);
                this.Data.AddRange(Avatar.Avatar.ToBytes);
                this.Data.AddInt(this.Device.State == State.WAR_EMODE ? 1 : 0);
                this.Data.AddInt(0);

                this.Data.AddLong(1462629754000);
                this.Data.AddLong(1462629754000);
                this.Data.AddLong(1462629754000);*/
            }
        }
    }
}
