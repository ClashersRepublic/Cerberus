using System;
using CRepublic.Royale.Core.Database;
using CRepublic.Royale.Extensions;
using CRepublic.Royale.Files;
using CRepublic.Royale.Logic.Enums;
using CRepublic.Royale.Packets;

namespace CRepublic.Royale.Core
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
