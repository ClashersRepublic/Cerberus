using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Logic;
using Republic.Magic.Logic.Components;

namespace Republic.Magic.Packets.Commands.Client
{
    internal class Change_Weapon_Heading : Command
    {
        internal int BuildingID;
        internal int Tick;
        public Change_Weapon_Heading(Reader reader, Device client, int id) : base(reader, client, id)
        {

        }

        internal override void Decode()
        {
            this.BuildingID = this.Reader.ReadInt32();
            this.Reader.ReadByte();
            this.Reader.ReadInt32();
            this.Reader.ReadString();
            this.Reader.ReadByte();
            this.Tick = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            var Object = this.Device.Player.Avatar.Variables.IsBuilderVillage ? this.Device.Player.GameObjectManager.GetBuilderVillageGameObjectByID(this.BuildingID) : this.Device.Player.GameObjectManager.GetGameObjectByID(this.BuildingID);
            if (Object?.GetComponent(1, true) == null)
                return;
            var a = ((Combat_Component)Object.GetComponent(1, false));
            if (a.AimRotateStep)
            {
                a.AimAngle = a.AimAngle == 360 ? 45 : a.AimAngle + 45;
                a.AimAngleDraft = a.AimAngleDraft == 360 ? 45 : a.AimAngleDraft + 45;
            }

            if (a.AltDirectionMode)
            {
                a.TrapDirection = a.TrapDirection == 4 ? 0 : a.TrapDirection++;
                a.TrapDirectionDraft = a.TrapDirectionDraft == 4 ? 0 : a.TrapDirectionDraft++;
            }
        }
    }
}
