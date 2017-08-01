using System;
using System.IO;
using Savage.Magic.Core;

using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Logic.StreamEntries;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.Commands.Client
{
    internal class RequestAllianceUnitsCommand : Command
    {
        public byte FlagHasRequestMessage;
        public string Message;
        public int MessageLength;
        public string Message2;

        public RequestAllianceUnitsCommand(PacketReader br)
        {
            br.ReadUInt32WithEndian();
            FlagHasRequestMessage = br.ReadByte();
            Message = br.ReadString();
            Message2 = br.ReadString();
        }

        public override void Execute(Level level)
        {
        }
    }
}
