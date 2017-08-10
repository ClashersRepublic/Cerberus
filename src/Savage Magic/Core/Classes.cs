using System;
using CRepublic.Magic.Core.Database;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Files;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Packets;

namespace CRepublic.Magic.Core
{
    internal static class Classes 
    {

        internal static void Initialize()
        {
            MessageFactory.Initialize();
            CSV.Initialize();
            Home.Initialize();
            NPC.Initialize();
            Fingerprint.Initialize();
            MySQL_V2.GetAllSeed();
            
        }
    }
}
