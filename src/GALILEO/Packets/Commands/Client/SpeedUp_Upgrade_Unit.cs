using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Structure;

namespace BL.Servers.CoC.Packets.Commands.Client
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
            var Object = this.Device.Player.GameObjectManager.GetGameObjectByID(this.BuildingId);
            if (Object == null|| Object.ClassId != 0 && Object.ClassId != 7)
                return;

            var upgradeComponent = ((ConstructionItem)Object).GetUnitUpgradeComponent(false);
            if (upgradeComponent?.GetUnit == null)
                return;
            upgradeComponent.SpeedUp();
        }
    }
}
