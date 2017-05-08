using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Commands.Server
{
    using BL.Servers.CR.Extensions.List;

    internal class Name_Change_Callback : Command
    {
        public string Name = string.Empty;
        public string Previous = string.Empty;

        public Name_Change_Callback(Device _Client) : base(_Client)
        {
            this.Identifier = 3;
        }
        
        internal override void Decode()
        {
            this.Previous = this.Name;
            this.Name = this.Reader.ReadString();
        }
        
        internal override void Encode()
        {
            this.Data.AddString(this.Device.Player.Avatar.Username);
            this.Data.AddInt(0);
            this.Data.AddInt(4);
            this.Data.AddInt(-1);
        }
    }
}
