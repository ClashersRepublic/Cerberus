using System;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions.Binary;

namespace CRepublic.Magic.Packets.Messages.Client
{
   internal class Keep_Alive : Message
    {
        public Keep_Alive(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Keep_Alive.
        }

        internal override void Process()
        {
            this.Device.LastKeepAlive = DateTime.Now;
            this.Device.NextKeepAlive = this.Device.LastKeepAlive.AddSeconds(30);
            this.Device.KeepAlive.Send();
        }
    }
}
