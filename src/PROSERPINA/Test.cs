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
            //this.Uncompress("06 00 3F 01 00 00 01 00 00 00 02 00 00 01 00 00 00 0E 00 00 01 00 00 00 82 01 00 00 01 00 00 00 81 01 00 00 01 00 00 00 04 00 00 01 00 00 00 00 00 00 0E 00 AC 49 2B 00 00 0E AB A4 E2 0A 0E AB A4 E2 0A 0E AB A4 E2 0A 00 00 00 00 7F 01 00 00 00 00 00 00 00 00 1F 00 00 00 00 00 07 08 05 01 A9 01 05 05 A9 01 05 1D 88 88 D5 44 05 0D 00 05 0E 00 05 03 01 05 10 01 05 02 01 00 03 3C 07 06 3C 08 06 3C 09 06 00 02 05 0B 1F 05 08 06 05 1A 00 02 1A 01 02 1C 01 02 1C 00 01 1A 0D 02 00 A4 01 A4 01 00 01 00 00 00 00 00 00 00 00 01 00 00 04 0E AB A4 E2 0A 0E AB A4 E2 0A 0E AB A4 E2 0A 00 00 00 00 7F 01 00 00 00 00 00 00 00 00 1F 00 00 00 00 00 07 08 05 01 A9 01 05 05 A9 01 05 1D 88 88 D5 44 05 0D 00 05 0E 00 05 03 01 05 10 01 05 02 01 00 03 3C 07 06 3C 08 06 3C 09 06 00 02 05 0B 1F 05 08 06 05 1A 00 02 1A 01 02 1C 01 02 1C 00 01 1A 0D 02 00 A4 01 A4 01 00 01 00 00 00 00 00 00 00 00 01 00 00 00".Replace(" ", ""));        
        }

        internal void Uncompress(string Hexa)
        {
            Reader br = new Reader(Hexa.HexaToBytes());

            //br.ReadInt32();
            //br.ReadInt32();
            //br.ReadInt32();
            //br.ReadInt32();

            //Console.WriteLine(BitConverter.ToString(ZlibStream.UncompressBuffer(br.ReadFully())).Replace("-",""));

            var data =  br.ReadFully();

            Console.WriteLine(BitConverter.ToString(data).Replace("-", ""));
            Console.WriteLine(Encoding.UTF8.GetString(data));

        }
    }
}