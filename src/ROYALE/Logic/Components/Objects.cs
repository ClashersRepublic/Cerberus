using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Logic.Components
{
    internal class Objects
    {
        internal Player Player;

        internal Objects(Player Player)
        {
            this.Player = Player;
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> _Packet = new List<byte>();

                return _Packet.ToArray();
            }
        }
    }
}
