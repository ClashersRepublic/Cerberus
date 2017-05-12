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
            this.Uncompress("84-03-84-03-00-1F-01-1A-03-FF-FF-FF-FF".Replace("-", ""));
        }

        internal void Uncompress(string Hexa)
        {
            Reader br = new Reader(Hexa.HexaToBytes());

            Console.WriteLine(br.ReadRRInt32());
            Console.WriteLine(br.ReadRRInt32());
            Console.WriteLine(br.ReadInt16());

            Console.WriteLine(br.ReadRRInt32());
            Console.WriteLine(br.ReadRRInt32());
            Console.WriteLine(br.ReadRRInt32());

            var data =  br.ReadFully();

            Console.WriteLine(BitConverter.ToString(data).Replace("-", ""));
            Console.WriteLine(Encoding.UTF8.GetString(data));

        }
    }
}