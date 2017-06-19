using Republic.Magic.Extensions;
using Republic.Magic.Extensions.List;
using Republic.Magic.Logic;

namespace Republic.Magic.Packets.Messages.Server.Clans
{
    internal class Alliance_All_Stream_Entry : Message
    {

        internal Clan Alliance = null;
        internal Alliance_All_Stream_Entry(Device _Device) : base(_Device)
        {
            this.Identifier = 24311;
        }

        internal Alliance_All_Stream_Entry(Device _Device, Clan clan) : base(_Device)
        {
            this.Identifier = 24311;
            this.Alliance = clan;
        }

        internal override void Encode()
        {
            this.Data.AddInt(0);
            this.Data.AddRange(Alliance != null ? this.Alliance.Chats.ToBytes : Core.Resources.Clans.Get(this.Device.Player.Avatar.ClanId, Constants.Database).Chats.ToBytes);
        }
    }
}
