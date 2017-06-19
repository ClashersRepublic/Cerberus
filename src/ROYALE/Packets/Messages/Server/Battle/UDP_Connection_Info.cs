using CRepublic.Royale.Core;
using CRepublic.Royale.Extensions;
using CRepublic.Royale.Extensions.List;
using CRepublic.Royale.Logic;

namespace CRepublic.Royale.Packets.Messages.Server.Battle
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
            this.Data.AddVInt(9339);
            this.Data.AddString("217.182.63.73");
            this.Data.AddString(""); // Nonce
            this.Data.AddString(""); // Session Key
        }
    }
}
