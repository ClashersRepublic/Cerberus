using System;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Packets;

namespace CRepublic.Magic.Core.Network
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