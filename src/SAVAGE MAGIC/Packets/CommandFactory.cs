using System;
using System.Collections.Generic;
using System.IO;

namespace Savage.Magic.Network
{
    internal static class CommandFactory
    {
        static readonly Dictionary<uint, Type> m_vCommands;

        static CommandFactory()
        {
            m_vCommands = new Dictionary<uint, Type>
            {
            };
        }
    }
}
