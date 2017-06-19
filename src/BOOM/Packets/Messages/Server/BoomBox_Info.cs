using System;
namespace CRepublic.Boom.Packets.Messages.Server
{
    using CRepublic.Boom.Extensions.List;
    using CRepublic.Boom.Logic;

    internal class BoomBox_Info :Message
    {
        internal BoomBox_Info(Device Device) : base(Device)
        {
            this.Identifier = 24448;
        }

        internal override void Encode()
        {
            this.Data.AddString("Hi bitch");
        }
    }
}
