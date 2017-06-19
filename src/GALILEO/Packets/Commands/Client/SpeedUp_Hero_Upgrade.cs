using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Structure;

namespace CRepublic.Magic.Packets.Commands.Client
{
    internal class SpeedUp_Hero_Upgrade : Command
    {
        internal int BuildingId;
        internal uint Tick;
        public SpeedUp_Hero_Upgrade(Reader reader, Device client, int id) : base(reader, client, id)
        {

        }

        internal override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            this.Tick = this.Reader.ReadUInt32();
        }

        internal override void Process()
        {
            var ca = this.Device.Player.Avatar;
            var go = this.Device.Player.Avatar.Variables.IsBuilderVillage ? this.Device.Player.GameObjectManager.GetBuilderVillageGameObjectByID(BuildingId) : this.Device.Player.GameObjectManager.GetGameObjectByID(BuildingId);
            if (go != null)
            {
                var hero = ((ConstructionItem)go).GetHeroBaseComponent();
                hero?.SpeedUpUpgrade();
            }
        }
    }
}
