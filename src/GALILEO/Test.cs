using System;
using System.Text;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Extensions.List;

namespace BL.Servers.CoC
{ 
    internal class Test
    {

        internal Test()
        {
            this.Uncompress("");
        }
        internal void Uncompress(string Hexa)
        {
            Reader br = new Reader(Hexa.HexaToBytes());
            var data =  br.ReadFully();

            Console.WriteLine(BitConverter.ToString(data).Replace("-", ""));
            Console.WriteLine(Encoding.UTF8.GetString(data));
        
        }
    }
}