using System;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic;

namespace BL.Servers.CoC.Packets.Messages.Server.Battle
{
    internal class V2_Battle_Info : Message
    {
        public V2_Battle_Info(Device Device) : base(Device)
        {
            this.Identifier = 24340;
        }

        internal override void Encode()
        {
            this.Data.AddHexa(
                "00 00 00 00 00 00 00 00 00 04 36 9F BE 01 00 00 00 03 00 3F B2 BA 00 00 00 0B 55 4C 54 52 41 20 46 4F 52 43 45 5B 00 1A 5A 00 00 00 02 00 00 00 0D 01 81 EC E8 00 00 00 0D 01 81 EC E8 00 00 00 15 77 68 61 74 20 61 72 65 20 79 6F 75 E5 BC 84 E5 95 A5 E5 98 9E 00 00 00 02 00 98 EA 01 98 EA 01 98 EA 01 98 EA 01 00 B0 03"
                    .Replace(" ", ""));
        }
    }
}