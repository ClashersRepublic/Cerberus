using System.Collections.Generic;
using System.IO;
using Savage.Magic;

namespace Savage.Magic.Logic
{
    internal class BaseHome
    {
        public virtual void Decode(byte[] data)
        {
            // Space
        }

        public virtual byte[] Encode()
        {
            return new List<byte>().ToArray();
        }
    }
}
