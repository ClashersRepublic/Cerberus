using System.ComponentModel.DataAnnotations;
using System.Text;
using BL.Servers.CR.Extensions;
using BL.Servers.CR.Files;
using BL.Servers.CR.Files.CSV_Logic;
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
            //98 - 02 - AC - 02 - 00 - 05 - 07 - 1A - 06 - 7F - 00 - AC - B3 - 01 - 94 - C0 - 02 - FF - FF - FF - FF // Balloon
            //AD - 06 - 81 - 07 - 00 - 05 - 05 - 1A - 03 - 7F - 00 - 8C - F2 - 01 - 8B - EF - 02 - FF - FF - FF - FF // Giant

            //this.Uncompress("AD-06-81-07-00-05-05-1A-03-7F-00-8C-F2-01-8B-EF-02-FF-FF-FF-FF".Replace("-", ""));
            //this.Uncompress("98-02-AC-02-00-05-07-1A-06-7F-00-AC-B3-01-94-C0-02-FF-FF-FF-FF".Replace("-", ""));

            //this.Uncompress("94-01-A8-01-00-05-06-1A-0E-7F-00-B3-84-01-93-C0-02-01-BE-01-92-02-00-05-01-1A-01-7F-00-AC-B3-01-AB-B0-02-FF-FF-FF-FF".Replace("-", ""));

            //this.Uncompress("06 00 3F 01 00 00 01 00 00 00 02 00 00 01 00 00 00 0E 00 00 01 00 00 00 82 01 00 00 01 00 00 00 81 01 00 00 01 00 00 00 04 00 00 01 00 00 00 00 00 00 0E 00 AC 49 2B 00 00 0E AB A4 E2 0A 0E AB A4 E2 0A 0E AB A4 E2 0A 00 00 00 00 7F 01 00 00 00 00 00 00 00 00 1F 00 00 00 00 00 07 08 05 01 A9 01 05 05 A9 01 05 1D 88 88 D5 44 05 0D 00 05 0E 00 05 03 01 05 10 01 05 02 01 00 03 3C 07 06 3C 08 06 3C 09 06 00 02 05 0B 1F 05 08 06 05 1A 00 02 1A 01 02 1C 01 02 1C 00 01 1A 0D 02 00 A4 01 A4 01 00 01 00 00 00 00 00 00 00 00 01 00 00 04 0E AB A4 E2 0A 0E AB A4 E2 0A 0E AB A4 E2 0A 00 00 00 00 7F 01 00 00 00 00 00 00 00 00 1F 00 00 00 00 00 07 08 05 01 A9 01 05 05 A9 01 05 1D 88 88 D5 44 05 0D 00 05 0E 00 05 03 01 05 10 01 05 02 01 00 03 3C 07 06 3C 08 06 3C 09 06 00 02 05 0B 1F 05 08 06 05 1A 00 02 1A 01 02 1C 01 02 1C 00 01 1A 0D 02 00 A4 01 A4 01 00 01 00 00 00 00 00 00 00 00 01 00 00 00".Replace(" ", ""));        
        }

        internal void Uncompress(string Hexa)
        {
            Reader br = new Reader(Hexa.HexaToBytes());

            Console.WriteLine(br.ReadVInt());
            Console.WriteLine(br.ReadVInt());
            Console.WriteLine(br.ReadVInt());
            Console.WriteLine(br.ReadVInt());
            Console.WriteLine(br.ReadVInt());
            Console.WriteLine(br.ReadVInt()); // Card Type
            Console.WriteLine(br.ReadVInt()); // Card ID
            Console.WriteLine(br.ReadVInt());
            Console.WriteLine(br.ReadVInt());
            Console.WriteLine(br.ReadVInt()); // X
            Console.WriteLine(br.ReadVInt()); // Y

            var data =  br.ReadFully();

            Console.WriteLine(BitConverter.ToString(data).Replace("-", ""));
            Console.WriteLine(Encoding.UTF8.GetString(data));

        }
    }
}