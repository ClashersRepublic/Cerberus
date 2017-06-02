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
            this.Data.AddHexa("BB 91 01 00 00 00 0E 35 34 2E 32 30 32 2E 31 36 32 2E 31 33 37 00 00 00 0A 47 A4 E8 34 59 0F BC 37 59 96 00 00 00 2B 53 39 6C 74 77 73 77 77 64 61 37 6A 74 37 4D 36 4F 44 63 42 50 70 63 34 59 4E 66 38 55 6D 34 33 37 63 34 6C 65 68 6A 69 43 59 34".Replace(" ", ""));

            //this.Data.AddVInt(9339);
            //this.Data.AddString("217.182.63.73"); // Server IP
            //this.Data.AddHexa("47 A4 E8 34 59 0F BC 37 59 96".Replace(" ", "")); // Nonce
            //this.Data.AddString("S9ltwswwda7jt7M6ODcBPpc4YNf8Um437c4lehjiCY4"); // Session Key
        }
    }
}
