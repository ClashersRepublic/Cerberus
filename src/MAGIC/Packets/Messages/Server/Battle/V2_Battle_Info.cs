using System;
using CRepublic.Magic.Extensions.List;
using CRepublic.Magic.Logic;

namespace CRepublic.Magic.Packets.Messages.Server.Battle
{
    internal class V2_Battle_Info : Message
    {
        internal Level Enemy;
        public V2_Battle_Info(Device Device, Level Enemy) : base(Device)
        {
            this.Identifier = 24372;
            this.Enemy = Enemy;
        }

        internal override void Encode()
        {
            this.Data.AddInt(0);
            this.Data.AddByte(0);
            this.Data.AddInt(0);
            this.Data.AddHexa("04 36 9F BE");
            this.Data.AddBool(this.Enemy.Avatar.ClanId > 0);
            if (this.Enemy.Avatar.ClanId > 0)
            {
                this.Data.AddLong(this.Enemy.Avatar.ClanId);
                this.Data.AddString(this.Enemy.Avatar.Alliance_Name);
                this.Data.AddInt(this.Enemy.Avatar.Badge_ID);
                this.Data.AddInt(this.Enemy.Avatar.Alliance_Level);
            }
            this.Data.AddLong(this.Enemy.Avatar.UserId);
            this.Data.AddLong(this.Enemy.Avatar.UserId);
            this.Data.AddString(this.Enemy.Avatar.Name);
            
            this.Data.AddInt(2);
            this.Data.AddByte(0);
            this.Data.AddVInt(15000);
            this.Data.AddVInt(15000);
            this.Data.AddVInt(15000);
            this.Data.AddVInt(15000);
            this.Data.AddVInt(0);
            this.Data.AddVInt((int)TimeSpan.FromMinutes(4).TotalSeconds);

        }
    }
}