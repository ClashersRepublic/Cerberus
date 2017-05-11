using System.ComponentModel.DataAnnotations;
using System.Text;
using BL.Servers.CR.Extensions;
using BL.Servers.CR.Library.ZLib;

namespace BL.Servers.CR
{
    using System;
    using BL.Servers.CR.Extensions.Binary;
    using BL.Servers.CR.Extensions.List;
    internal class Test
    {

        internal Test()
        {
            this.Uncompress("00 21 7F 0B 00 00 00 00 02 02 01 7F 7F 7F 7F 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 07 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 04 34 89 96 CC 04 34 89 96 CC 04 34 89 96 CC 04 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 07 01 05 01 A4 01 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 01 02 01 01 7F 7F 00 34 89 96 CC 04 00 00 00 00 00 00 00 00 00 04 01 00 00 00 8E 02 F2 7D 00 00 06 06 23 01 23 01 23 01 23 01 23 00 23 00 01 00 01 00 00 01 05 00 05 01 05 02 05 03 05 04 05 05 00 0D A4 E2 01 9C 8E 03 00 00 C0 7C 00 A4 01 00 00 00 00 02 00 00 00 00 00 00 00 00 00 0D AC 36 A4 65 00 00 80 04 00 A4 01 00 00 00 00 01 00 00 00 00 00 00 00 00 00 0D AC 36 9C 8E 03 00 00 C0 7C 00 A4 01 00 00 00 00 01 00 00 00 00 00 00 00 00 00 0D A4 E2 01 A4 65 00 00 80 04 00 A4 01 00 00 00 00 02 00 00 00 00 00 00 00 00 00 0D A8 8C 01 B8 2E 00 00 80 04 00 A4 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 01 00 7F 00 00 00 00 7F 7F 00 00 00 01 00 00 00 00 00 7F 7F 7F 7F 7F 7F 7F 7F 00 0D A8 8C 01 88 C5 03 00 00 C0 7C 00 A4 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 04 00 01 01 01 02 04 05 00 7F 7F 00 00 00 05 00 00 00 00 00 7F 7F 7F 7F 7F 7F 7F 7F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 B8 15 B0 08 B8 15 B0 08 80 0F A0 25 00 00 00 00 00 00 00 A4 01 A4 01 00 00 00 00 00 00 00 A4 01 A4 01 00 00 00 00 00 00 00 A4 01 A4 01 00 00 00 00 00 00 00 A4 01 A4 01 00 00 00 00 00 00 00 A4 01 A4 01 00 00 00 00 00 00 00 A4 01 A4 01 03 00 03 00 00 FE 00 01 00 02 00 0E 00 82 01 00 81 01 00 04 00 00 00 05 06 02 02 04 02 01 03 00 00 00 00 00 00 00 06 09 01 01 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 0C 00 00 00 D4 E0 F2 7A 00 2A 00 2B".Replace(" ", ""));
        }

        internal void Uncompress(string Hexa)
        {
            Reader br = new Reader(Hexa.HexaToBytes());

            Console.WriteLine(br.ReadRRInt32());
            Console.WriteLine(br.ReadRRInt32());

            var data =  br.ReadFully();

            Console.WriteLine(BitConverter.ToString(data).Replace("-", ""));
            Console.WriteLine(Encoding.UTF8.GetString(data));

        }
    }
}