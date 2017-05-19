using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CoC.Logic.Structure.Slots.Items
{
    internal class Mail
    {
        internal Mail()
        {
            // Mail.
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> Packet = new List<byte>();



                return Packet.ToArray();
            }
        }
    }
}
