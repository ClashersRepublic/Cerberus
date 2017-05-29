using BL.Servers.CR.Core;
using BL.Servers.CR.Extensions;
using BL.Servers.CR.Extensions.List;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Messages.Server.Battle
{
    internal class UDP_Connection_Info : Message
    {
        internal byte[] Nonce;

        internal UDP_Connection_Info(Device Device) : base(Device)
        {
            this.Identifier = 24112;
            this.Nonce = Utils.CreateByteArray(24);
        }

        internal override void Encode()
        {
            this.Data.AddVInt(Constants.ServerPort);
            this.Data.AddString(""); // Server IP or SesionID?
            this.Data.AddByteArray(this.Nonce); // Nonce
        }
    }
}
