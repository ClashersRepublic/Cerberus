using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Files;
using BL.Servers.CoC.Files.CSV_Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Logic.Structure;
using Newtonsoft.Json.Linq;

namespace BL.Servers.CoC.Logic.Components
{
    internal class Combat_Component : Component
    {
        public Combat_Component(ConstructionItem ci, Level level) : base(ci)
        {
            var bd = (Buildings)ci.Data;
            if (bd.AmmoCount >  0)
            {
                this.Ammo = bd.AmmoCount;
            }
            if  (bd.AltAttackMode)
            {
                this.AltAttackMode = true;
            }
            if (bd.AimRotateStep > 0)
            {
                this.AimRotateStep = true;
            }
        }

        internal override int Type => 1;
        internal int Ammo = -1;
        internal int GearUp = -1;
        internal int Aim_Angle;
        internal int Aim_Angle_Draft;
        internal bool AltAttackMode = false;
        internal bool AimRotateStep = false;
        internal bool Attack_Mode = false;
        internal bool Attack_Mode_Draft = false;

        internal void FillAmmo()
        {
            var ca = this.GetParent.Level.Avatar;
            var bd = (Buildings)this.GetParent.Data;
            var rd = CSV.Tables.Get(Gamefile.Resources).GetData(bd.AmmoResource);

            if (ca.HasEnoughResources(rd.GetGlobalID(), bd.AmmoCost[0]))
            {
                ca.Resources.ResourceChangeHelper(rd.GetGlobalID(), bd.AmmoCost[0]);
                this.Ammo = bd.AmmoCount;
            }
        }
        internal override void Load(JObject jsonObject)
        {
            if (jsonObject["gear"] != null)
            {
                this.GearUp = jsonObject["gear"].ToObject<int>();
            }

            if (jsonObject["ammo"] != null)
            {
                this.Ammo = jsonObject["ammo"].ToObject<int>();
            }   
            if (jsonObject["attack_mode"] != null)
            {
                this.AltAttackMode = true;
                this.Attack_Mode = jsonObject["attack_mode"].ToObject<bool>();
                this.Attack_Mode_Draft = jsonObject["attack_mode_draft"].ToObject<bool>();
            }
            if (jsonObject["aim_angle"] != null)
            {
                this.AimRotateStep = true;
                this.Aim_Angle = jsonObject["aim_angle"].ToObject<int>();
                this.Aim_Angle_Draft = jsonObject["aim_angle_draft"].ToObject<int>();
            }
        }
        internal override JObject Save(JObject jsonObject)
        {
            if (this.GearUp >= 0)
                jsonObject.Add("gear", this.GearUp);
            if (this.Ammo >= 0)
            jsonObject.Add("ammo", this.Ammo);

            if (this.AltAttackMode)
            {
                jsonObject.Add("attack_mode", this.Attack_Mode);
                jsonObject.Add("attack_mode_draft", this.Attack_Mode_Draft);
            }
            if (this.AimRotateStep)
            {
                jsonObject.Add("aim_angle", this.Aim_Angle);
                jsonObject.Add("aim_angle_draft", this.Aim_Angle_Draft);
            }

            return jsonObject;
        }
    }
}
