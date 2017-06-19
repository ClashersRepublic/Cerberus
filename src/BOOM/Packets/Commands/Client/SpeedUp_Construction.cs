namespace CRepublic.Boom.Packets.Commands.Client
{
    using CRepublic.Boom.Extensions.Binary;
    using CRepublic.Boom.Logic;
    using CRepublic.Boom.Logic.Structure;

    internal class SpeedUp_Construction : Command
    {
        internal int BuildingId;

        public SpeedUp_Construction(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            var go = this.Device.Player.GameObjectManager.GetGameObjectByID(this.BuildingId);
            if (go != null)
            {
                if (go.ClassId == 0 || go.ClassId == 4)
                {
                    (go as ConstructionItem).SpeedUpConstruction();
                }
            }
        }
    }
}