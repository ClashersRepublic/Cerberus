using System.Windows;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;

namespace BL.Servers.CoC.Packets.Commands.Client
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
            Vector Building1Position = this.Device.Player.GameObjectManager.GetGameObjectByID(Building1).GetPosition();
            Vector Building2Position = this.Device.Player.GameObjectManager.GetGameObjectByID(Building2).GetPosition();

            this.Device.Player.GameObjectManager.GetGameObjectByID(Building1).SetPositionXY(Building2Position);
            this.Device.Player.GameObjectManager.GetGameObjectByID(Building2).SetPositionXY(Building1Position);
        }
    }
}
