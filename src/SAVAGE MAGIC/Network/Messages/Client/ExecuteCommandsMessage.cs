using System;
using System.IO;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    // Packet 14102
    internal class ExecuteCommandsMessage : Message
    {
        public ExecuteCommandsMessage(ClashOfClans.Client client, PacketReader br) : base(client, br)
        {
        }

        public uint Checksum;
        public byte[] NestedCommands;
        public uint NumberOfCommands;
        public uint Subtick;

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(Data)))
            {
                Subtick = br.ReadUInt32WithEndian();
                Checksum = br.ReadUInt32WithEndian();
                NumberOfCommands = br.ReadUInt32WithEndian();

                if (NumberOfCommands > 0 && NumberOfCommands < 120)
                    NestedCommands = br.ReadBytes(Length- 12);
            }
        }

        public override void Process(Level level)
        {
            level.Tick();

            if (NumberOfCommands > 0 && NumberOfCommands < 120)
            {
                using (var reader = new PacketReader(new MemoryStream(NestedCommands)))
                {
                    for (var i = 0; i < NumberOfCommands; i++)
                    {
                        var cmd = (Command)CommandFactory.Read(reader, 0);
                        if (cmd != null)
                        {
                            try { cmd.Execute(level); }
                            catch (Exception ex)
                            {
                                ExceptionLogger.Log(ex, $"Exception while processing command {cmd.GetType()}");
                            }
                        }
                        else break;
                    }
                }
            }
        }
    }
}
