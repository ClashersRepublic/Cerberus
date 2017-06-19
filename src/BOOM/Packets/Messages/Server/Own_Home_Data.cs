using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Boom.Extensions.List;
using CRepublic.Boom.Logic;

namespace CRepublic.Boom.Packets.Messages.Server
{
    internal class Own_Home_Data : Message
    {
        internal Own_Home_Data(Device Device) : base(Device)
        {
            this.Identifier = 24101;
        }

        internal override void Encode()
        {
            this.Device.Player.Home.Village = this.Device.Player.SaveToJSON();
            this.Data.AddRange(this.Device.Player.Home.ToBytes);
            this.Data.AddRange(this.Device.Player.Avatar.ToBytes);
            
            
            /* this.Data.AddInt(0);
            this.Data.AddInt(-1);

            this.Data.AddRange(this.Device.Player.Objects.ToBytes);
            this.Data.AddRange(this.Device.Player.ToBytes);
            this.Data.AddInt(this.Device.State == State.WAR_EMODE ? 1 : 0);
            this.Data.AddInt(0);*/
        }
    }
}
