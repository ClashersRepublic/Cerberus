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
            this.Data.AddByte(0);
            this.Data.AddInt(4);
            this.Data.AddInt(916438529);
            this.Data.AddLong(this.Enemy.Avatar.Amical_ID); //Alliance ID
            this.Data.AddString(this.Enemy.Avatar.Alliance_Name);
            this.Data.AddInt(0);
            this.Data.AddInt(2);
            this.Data.AddLong(this.Enemy.Avatar.UserId);
            this.Data.AddLong(this.Enemy.Avatar.UserId);
            this.Data.AddString(this.Enemy.Avatar.Name);
            
            //Some dicky stuff
            this.Data.AddInt(2);
            this.Data.AddByte(0);
            this.Data.AddHexa("98EA01");
            this.Data.AddHexa("98EA01");
            this.Data.AddHexa("98EA01");
            this.Data.AddHexa("98EA01");
            this.Data.AddHexa("009103"); //009203 also exist
        }
    }
}