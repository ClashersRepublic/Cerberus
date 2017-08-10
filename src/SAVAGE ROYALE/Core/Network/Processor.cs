using System;
using CRepublic.Royale.Extensions;
using CRepublic.Royale.Packets;

namespace CRepublic.Royale.Core.Network
{
    internal static class Processor
    {
        internal static void Recept(this Message Message)
        {
            Message.Encrypt();
            Message.Decode();
            Message.Process();
        }

        /*internal static Command Handle(this Command Command)
        {
            Command.Encode();

            return Command;
        }*/
    }
}