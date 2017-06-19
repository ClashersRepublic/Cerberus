using CRepublic.Boom.Extensions.Binary;
using CRepublic.Boom.Extensions.List;

namespace CRepublic.Boom
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