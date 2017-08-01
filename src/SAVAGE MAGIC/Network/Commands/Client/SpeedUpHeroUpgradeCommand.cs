using System.IO;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class SpeedUpHeroUpgradeCommand : Command
    {
        internal int m_vBuildingId;
        internal int m_vUnknown1;

        public SpeedUpHeroUpgradeCommand(PacketReader br)
        {
            m_vBuildingId = br.ReadInt32();
            m_vUnknown1 = br.ReadInt32();
        }

        public override void Execute(Level level)
        {
            GameObject gameObjectById = level.GameObjectManager.GetGameObjectByID(m_vBuildingId);
            if (gameObjectById == null)
                return;
            HeroBaseComponent heroBaseComponent = ((ConstructionItem) gameObjectById).GetHeroBaseComponent(false);
            if (heroBaseComponent == null)
                return;
            heroBaseComponent.SpeedUpUpgrade();
        }
    }
}
