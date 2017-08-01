using System.IO;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class LoadTurretCommand : Command
    {
        public int m_vBuildingId;
        public uint m_vUnknown1;
        public uint m_vUnknown2;

        public LoadTurretCommand(PacketReader br)
        {
            m_vUnknown1 = br.ReadUInt32();
            m_vBuildingId = br.ReadInt32();
            m_vUnknown2 = br.ReadUInt32();
        }

        public override void Execute(Level level)
        {
            GameObject gameObjectById = level.GameObjectManager.GetGameObjectByID(m_vBuildingId);
            if ((gameObjectById != null ? gameObjectById.GetComponent(1, true) : (Component) null) == null)
              return;
            ((CombatComponent) gameObjectById.GetComponent(1, true)).FillAmmo();
        }
    }
}
