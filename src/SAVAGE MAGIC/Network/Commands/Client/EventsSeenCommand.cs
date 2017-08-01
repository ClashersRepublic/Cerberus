using System.IO;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class EventsSeenCommand : Command
    {
        public int Tick;
        public int UnknownID;

        public EventsSeenCommand(PacketReader br)
        {
            Tick = br.ReadInt32();
            UnknownID = br.ReadInt32();
        }

        public override void Execute(Level level)
        {
        }
    }
}