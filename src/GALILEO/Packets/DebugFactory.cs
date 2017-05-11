using System;
using System.Collections.Generic;

namespace BL.Servers.CoC.Packets
{
    internal class DebugFactory
    {
        internal const char Delimiter = '/';

        public static Dictionary<string, Type> Debugs;

        public DebugFactory()
        {
            Debugs = new Dictionary<string, Type>
            {
                // DebugFactory.
            };
        }
    }
}