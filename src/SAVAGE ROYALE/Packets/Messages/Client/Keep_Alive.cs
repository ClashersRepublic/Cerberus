using System;
using CRepublic.Royale.Core.Network;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Extensions.Binary;

namespace CRepublic.Royale.Packets.Messages.Client
{
   internal class Keep_Alive : Message
    {
        public Keep_Alive(Device Device) : base(Device)
        {
        }

        internal override void Process()
        {
            this.Device.LastKeepAlive = DateTime.Now;
            this.Device.NextKeepAlive = this.Device.LastKeepAlive.AddSeconds(30);
            this.Device.Keep_Alive.Send();
        }
    }
}
