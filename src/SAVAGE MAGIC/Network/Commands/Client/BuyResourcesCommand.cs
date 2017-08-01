using System;
using System.IO;
using Magic.ClashOfClans.Core;
using Magic.Files.Logic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class BuyResourcesCommand : Command
    {
        internal object m_vCommand;
        internal bool m_vIsCommandEmbedded;
        internal int m_vResourceCount;
        internal int m_vResourceId;
        internal int Unknown1;

        public BuyResourcesCommand(PacketReader br)
        {
            m_vResourceId = br.ReadInt32();
            m_vResourceCount = br.ReadInt32();
            m_vIsCommandEmbedded = br.ReadBoolean();
            if (m_vIsCommandEmbedded)
            {
                Depth = Depth + 1;

                if (Depth >= 10)
                {
                    Console.WriteLine("Detected Exploit.");
                    return;
                }
                else
                {
                    m_vCommand = CommandFactory.Read(br, Depth);
                }
            }
            Unknown1 = br.ReadInt32();
        }

        public override void Execute(Level level)
        {
            if (Depth >= 10)
            {
                //TODO: Block Exploitor's IP.
                ResourcesManager.DropClient(level.Client.GetSocketHandle());
            }
            else
            {
                ResourceData dataById = (ResourceData)CsvManager.DataTables.GetDataById(m_vResourceId);
                if (dataById == null || m_vResourceCount < 1 || dataById.PremiumCurrency)
                    return;

                Avatar avatar = level.Avatar;
                int resourceDiamondCost = GamePlayUtil.GetResourceDiamondCost(m_vResourceCount, dataById);
                if (m_vResourceCount > avatar.GetUnusedResourceCap(dataById) || !avatar.HasEnoughDiamonds(resourceDiamondCost))
                    return;

                avatar.UseDiamonds(resourceDiamondCost);
                avatar.CommodityCountChangeHelper(0, (Data)dataById, m_vResourceCount);
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
}
