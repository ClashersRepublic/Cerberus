using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Components;

namespace BL.Servers.CoC.Packets.Commands.Client
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
            var Object = this.Device.Player.GameObjectManager.GetGameObjectByID(this.BuildingID);
            if (Object?.GetComponent(1, true) == null)
                return;
            var a = ((Combat_Component)Object.GetComponent(1, true));
            if (a.AimRotateStep)
            {
                a.Aim_Angle = a.Aim_Angle == 360 ? 45 : a.Aim_Angle + 45;
                a.Aim_Angle_Draft = a.Aim_Angle_Draft == 360 ? 45 : a.Aim_Angle_Draft + 45;
            }
        }
    }
}
