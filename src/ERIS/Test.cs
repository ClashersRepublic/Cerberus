using BL.Servers.BB.Extensions.Binary;
using BL.Servers.BB.Extensions.List;

namespace BL.Servers.BB
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
        }
    }
}