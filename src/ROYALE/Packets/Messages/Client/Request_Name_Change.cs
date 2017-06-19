using CRepublic.Royale.Core.Network;
using CRepublic.Royale.Extensions.Binary;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Packets.Messages.Server;

namespace CRepublic.Royale.Packets.Messages.Client
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
            this.Device.Player.Username = this.Name;

            new Name_Change_Response(Device, this.Name).Send();
        }
    }
}
