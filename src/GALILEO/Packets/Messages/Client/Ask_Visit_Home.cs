using Republic.Magic.Core;
using Republic.Magic.Core.Networking;
using Republic.Magic.Extensions;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Logic;
using Republic.Magic.Packets.Messages.Server;
using Republic.Magic.Packets.Messages.Server.Clans;

namespace Republic.Magic.Packets.Messages.Client
{
    internal class Ask_Visit_Home : Message
    {
        internal long AvatarId;

        public Ask_Visit_Home(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Decode()
        {
            this.AvatarId = this.Reader.ReadInt64();
        }

        internal override void Process()
        {
            var target = Resources.Players.Get(this.AvatarId, Constants.Database, false);
            if (target != null)
            {
                new Visit_Home_Data(this.Device, target).Send();

                if (this.Device.Player.Avatar.ClanId > 0)
                {
                    new Alliance_All_Stream_Entry(this.Device).Send();
                }
            }
            else
                new Own_Home_Data(this.Device).Send();
        }
    }
}
