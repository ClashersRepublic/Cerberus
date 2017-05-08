using BL.Servers.CR.Core;
using BL.Servers.CR.Extensions.List;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Messages.Server.Battle
{
    internal class UDP_Connection_Info : Message
    {
        internal UDP_Connection_Info(Device Device) : base(Device)
        {
            this.Identifier = 24112;
        }

        internal override void Encode()
        {
            this.Data.AddVInt(Constants.ServerPort);
            this.Data.AddString(Constants.ServerAddr);
            this.Data.AddString("Session ID"); // Session ID
            this.Data.AddString(""); // Nonce
        }
    }
}
