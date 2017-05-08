using System;
using BL.Servers.CR.Extensions.List;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Messages.Server
{
    internal class Own_Home_Data : Message
    {
        internal Own_Home_Data(Device Device) : base(Device)
        {
            this.Identifier = 24101;
        }

        internal override void Encode()
        {
            int TimeStamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            this.Data.AddRange(this.Device.Player.Avatar.Components);
            this.Data.AddRange(this.Device.Player.Avatar.Profile);
            this.Data.AddVInt(TimeStamp);

            this.Data.AddVInt(TimeStamp);

            this.Data.AddHexa("0B AC A3 2C".Replace(" ", ""));
        }
    }
}
