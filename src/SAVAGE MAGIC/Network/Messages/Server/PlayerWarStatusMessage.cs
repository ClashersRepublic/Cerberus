using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Network;

namespace Magic.Packets.Messages.Server
{
    internal class PlayerWarStatusMessage : Message
    {
        internal int m_vStatus;

        public override void Encode()
        {
            List<byte> pack = new List<byte>();
            pack.AddInt32(14);
            pack.AddInt32(m_vStatus);
            pack.AddInt32(0);
            Encrypt(pack.ToArray());
        }
    }
}
