using CRepublic.Magic.Core;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.List;
using CRepublic.Magic.Logic;

namespace CRepublic.Magic.Packets.Messages.Server
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
            this.Player = this.UserID == this.Device.Player.Avatar.UserId ? this.Device.Player : Resources.Players.Get(UserID, false);

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
