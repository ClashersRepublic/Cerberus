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
            this.Uncompress(
                "08A53AAC04000AA38909BC33001E919133B82E000000070C0501B97E0502870E050300050400050CBF13050D00050E00050FAF100516811605199BEFFF8601051A0B051D8688D5440000000505068B3E0507AF05050B1E051408051B0B3D1A00001A01001A02001A03001A04001A05001A06001A07001A08001A09001A0A001A0B001A0C001A0D011A0E001A0F001A10001A11001A12001A13001A14001A15001A16001A17001A18001A19001A1A011A1B001A1C001A1D001A1E001A1F001A20001A22001A23001A25001A27001A2A0E1B00001B01001B02001B03001B04001B05001B06001B07001B08001B09001B0A001C00011C01001C02001C03001C04001C05001C06001C07001C08001C09001C0A001C0B00000A008E30AB10008D17A4147DB901220004");
        }
        /*this.Reader.ReadRRInt32();
        this.Reader.ReadRRInt32();

        this.Reader.ReadInt16();

        this.Reader.Read();

        this.TroopID = this.Reader.ReadRRInt32();

        this.Reader.Read();

        this.Reader.ReadRRInt32();
        this.Reader.ReadRRInt32();*/
        internal void Uncompress(string Hexa)
        {
            Reader br = new Reader(Hexa.HexaToBytes());
            
            Console.WriteLine($"Arena {br.ReadByte()}");
            Console.WriteLine($"Trophy {br.ReadRRInt32()}");
            Console.WriteLine($"Unknown RRInt32 {br.ReadRRInt32()}");
            Console.WriteLine($"Unknown RRInt32 {br.ReadRRInt32()}");

            var data =  br.ReadFully();

            Console.WriteLine(BitConverter.ToString(data).Replace("-", ""));
            Console.WriteLine(Encoding.UTF8.GetString(data));

        }
    }
}