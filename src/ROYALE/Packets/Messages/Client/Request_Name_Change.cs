using BL.Servers.CR.Core.Network;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;
using BL.Servers.CR.Packets.Messages.Server;

namespace BL.Servers.CR.Packets.Messages.Client
{
    internal class Request_Name_Change : Message
    {
        internal string Name;

        public Request_Name_Change(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Request_Name_Change.
        }

        internal override void Decode()
        {
            this.Name = this.Reader.ReadString();
        }

        internal override void Process()
        {
            this.Device.Player.Avatar.Username = this.Name;

            new Name_Change_Response(Device, this.Name).Send();
        }
    }
}
