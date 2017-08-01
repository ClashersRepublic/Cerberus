using System;
using System.IO;
using Magic.ClashOfClans.Core;

using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Logic.StreamEntries;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Commands.Client
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
