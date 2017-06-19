using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Packets.Messages.Server;


namespace CRepublic.Magic.Packets.Messages.Client
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
            new Name_Change_Response(this.Device, this.Name).Send();
        }
    }
}
