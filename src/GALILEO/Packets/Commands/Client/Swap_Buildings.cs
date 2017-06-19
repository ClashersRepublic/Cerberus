using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;

namespace CRepublic.Magic.Packets.Commands.Client
{
    internal class Swap_Buildings : Command
    {

        internal int Building1;
        internal int Building2;
        internal int Tick;

        public Swap_Buildings(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.Building1 = this.Reader.ReadInt32();
            this.Building2 = this.Reader.ReadInt32();
            this.Tick = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            var Building1Position = this.Device.Player.GameObjectManager.GetGameObjectByID(Building1);
            var Building2Position = this.Device.Player.GameObjectManager.GetGameObjectByID(Building2);

            if (Building1Position == null || Building2Position == null)
                return;

            this.Device.Player.GameObjectManager.GetGameObjectByID(Building1).SetPositionXY(Building2Position.GetPosition());
            this.Device.Player.GameObjectManager.GetGameObjectByID(Building2).SetPositionXY(Building1Position.GetPosition());
        }
    }
}
