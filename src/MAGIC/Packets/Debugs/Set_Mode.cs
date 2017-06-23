using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Magic.Logic;

namespace CRepublic.Magic.Packets.Debugs
{
    internal class Set_Mode : Debug
    {
        internal StringBuilder Help;

        public Set_Mode(Device device, params string[] Parameters) : base(device, Parameters)
        {

        }

        internal override void Process()
        {
            base.Process();
        }
    }
}
