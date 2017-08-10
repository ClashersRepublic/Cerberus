using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRepublic.Royale.Logic
{
    internal partial class Device
    {
        public byte[] PublicKey { get; set; }
        public byte[] RNonce { get; set; }
        public byte[] SessionKey { get; set; }
        public byte[] SharedKey { get; set; }
        public byte[] SNonce { get; set; }
    }
}
