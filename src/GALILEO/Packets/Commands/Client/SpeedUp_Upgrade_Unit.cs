using Republic.Magic.Extensions.Binary;
using Republic.Magic.Logic;
using Republic.Magic.Logic.Structure;

namespace Republic.Magic.Packets.Commands.Client
{
    internal class SpeedUp_Upgrade_Unit : Command
    {
        internal int BuildingId;
        internal int Tick;

        public SpeedUp_Upgrade_Unit(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            this.Tick = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            var Object = this.Device.Player.Avatar.Variables.IsBuilderVillage ? this.Device.Player.GameObjectManager.GetBuilderVillageGameObjectByID(this.BuildingId) : this.Device.Player.GameObjectManager.GetGameObjectByID(this.BuildingId);

            if (Object == null|| Object.ClassId != 0 && Object.ClassId != 7)
                return;

            var upgradeComponent = ((ConstructionItem)Object).GetUnitUpgradeComponent();
            if (upgradeComponent?.GetUnit == null)
                return;
            upgradeComponent.SpeedUp();
        }
    }
}
