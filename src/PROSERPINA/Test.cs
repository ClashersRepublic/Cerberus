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
            this.Uncompress("00-00-55-8E".Replace("-", ""));        
        }

        internal void Uncompress(string Hexa)
        {
            Reader br = new Reader(Hexa.HexaToBytes());

            Console.WriteLine(br.ReadInt32());

            var data =  br.ReadFully();

            Console.WriteLine(BitConverter.ToString(data).Replace("-", ""));
            Console.WriteLine(Encoding.UTF8.GetString(data));

        }
    }
}