using System;
using CRepublic.Magic.Core.Network;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Extensions.Binary;

namespace CRepublic.Magic.Packets.Messages.Client
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
