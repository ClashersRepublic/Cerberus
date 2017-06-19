using Republic.Magic.Extensions.List;
using Republic.Magic.Logic;
using Republic.Magic.Logic.Enums;

namespace Republic.Magic.Packets.Messages.Server.Clans
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
