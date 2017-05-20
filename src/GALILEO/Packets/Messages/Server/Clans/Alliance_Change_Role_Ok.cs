using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;

namespace BL.Servers.CoC.Packets.Messages.Server.Clans
{
    internal class Alliance_Change_Role_Ok : Message
    {
        internal long UserID;
        internal Role Role;

        internal Alliance_Change_Role_Ok(Device _Device) : base(_Device)
        {
            this.Identifier = 24306;
        }
        internal override void Encode()
        {
            this.Data.AddLong(this.UserID);
            this.Data.AddInt((int)this.Role);
        }
    }
}
