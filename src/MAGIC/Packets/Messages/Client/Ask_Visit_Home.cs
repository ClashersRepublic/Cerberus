using CRepublic.Magic.Core;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Packets.Messages.Server;
using CRepublic.Magic.Packets.Messages.Server.Clans;

namespace CRepublic.Magic.Packets.Messages.Client
{
    internal class Ask_Visit_Home : Message
    {
        internal long AvatarId;

        public Ask_Visit_Home(Device device) : base(device)
        {
        }

        internal override void Decode()
        {
            this.AvatarId = this.Reader.ReadInt64();
        }

        internal override void Process()
        {
            var target = Players.Get(this.AvatarId, false);
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
