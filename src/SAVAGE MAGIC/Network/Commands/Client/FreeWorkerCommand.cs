using System;
using System.IO;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class FreeWorkerCommand : Command
    {
        public int m_vTimeLeftSeconds;
        internal object m_vCommand;
        internal bool m_vIsCommandEmbedded;

        public FreeWorkerCommand(PacketReader br)
        {
            m_vTimeLeftSeconds = br.ReadInt32();
            m_vIsCommandEmbedded = br.ReadBoolean();
            if (!m_vIsCommandEmbedded)
            {
                Depth = Depth + 1;

                if (Depth >= 10)
                {
                    Console.WriteLine("Detected Exploit");
                    return;
                }
                else
                {
                    m_vCommand = CommandFactory.Read(br, Depth);
                }
            }
        }

        public override void Execute(Level level)
        {
            if (Depth >= 10)
            {
                //TODO: Block Exploitor's IP.
                ResourcesManager.DropClient(level.Client.GetSocketHandle());
            }
            if (level.WorkerManager.GetFreeWorkers() != 0)
                return;
            Depth = 0;
            level.WorkerManager.FinishTaskOfOneWorker();
            if (!m_vIsCommandEmbedded)
                return;

            if (m_vIsCommandEmbedded && m_vCommand != null)
            {
                var cmd = (Command)m_vCommand;
                cmd.Execute(level);
            }
        }
    }
}
