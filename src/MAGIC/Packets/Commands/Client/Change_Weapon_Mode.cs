using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Components;

namespace CRepublic.Magic.Packets.Commands.Client
{
    internal class Change_Weapon_Mode : Command
    {
        internal int BuildingID;
        internal int Tick;
        public Change_Weapon_Mode(Reader reader, Device client, int id) : base(reader, client, id)
        {

        }

        internal override void Decode()
        {

            this.BuildingID = this.Reader.ReadInt32();
            this.Reader.ReadByte();
            this.Reader.ReadInt32();
            this.Tick = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            var Object = this.Device.Player.Avatar.Variables.IsBuilderVillage ? this.Device.Player.GameObjectManager.GetBuilderVillageGameObjectByID(this.BuildingID) : this.Device.Player.GameObjectManager.GetGameObjectByID(this.BuildingID);
            if (Object?.GetComponent(1, true) == null)
                return;
            var a = ((Combat_Component) Object.GetComponent(1, false));
            if (a.AltAttackMode)
            {
                a.AttackMode = !a.AttackMode;
                a.AttackModeDraft = !a.AttackModeDraft;
            }

            if (a.AltTrapAttackMode)
            {
                a.AirMode = !a.AirMode;
                a.AirModeDraft = !a.AirModeDraft;
            }
        }
    }
}
