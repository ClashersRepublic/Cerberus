using BL.Servers.CoC.Core;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Packets.Messages.Server.Clans;

namespace BL.Servers.CoC.Packets.Messages.Client.Clans
{
    internal class Ask_Alliance : Message
    {
        internal long ClanID;

        public Ask_Alliance(Device Device, Reader Reader) : base(Device, Reader)
        {

        }

        internal override void Decode()
        {
            this.ClanID = this.Reader.ReadInt64();
        }

        internal override void Process()
        {
            new Alliance_Data(this.Device) {Clan = Resources.Clans.Get(this.ClanID)}.Send();
        }
    }
}