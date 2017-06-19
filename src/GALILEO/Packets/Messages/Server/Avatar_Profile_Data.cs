using Republic.Magic.Core;
using Republic.Magic.Extensions;
using Republic.Magic.Extensions.List;
using Republic.Magic.Logic;

namespace Republic.Magic.Packets.Messages.Server
{
    internal class Avatar_Profile_Data : Message
    {
        internal Level Player;
        internal long UserID;

        public Avatar_Profile_Data(Device Device) : base(Device)
        {
            this.Identifier = 24334;
        }
        internal override void Encode()
        {
            this.Player = this.UserID == this.Device.Player.Avatar.UserId ? this.Device.Player : Resources.Players.Get(UserID, Constants.Database, false);

            this.Data.AddRange(this.Player.Avatar.ToBytes);
            this.Data.AddCompressed(this.Player.JSON, false);

            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddInt(0);

            this.Data.AddInt(0);
            this.Data.Add(0);
        }
    }
}
