using System;
using System.Collections.Generic;
using BL.Servers.CoC.Packets.Debugs;

namespace BL.Servers.CoC.Packets
{
    internal class DebugFactory
    {
        internal const string Delimiter = "/";

        public static Dictionary<string, Type> Debugs;

        public DebugFactory()
        {
            Debugs = new Dictionary<string, Type>
            {
                {"resource", typeof(Resource_Update)},
                {"stats", typeof(Statistics) }
            };
        }
    }
}