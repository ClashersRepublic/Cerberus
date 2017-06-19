using System;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.List;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;

namespace CRepublic.Magic.Packets.Messages.Server.Battle
{
    internal class Pc_Battle_Data : Message
    {
        internal Level Enemy = null;
        internal Battle_Mode BattleMode;

        public Pc_Battle_Data(Device Device) : base(Device)
        {
            this.Identifier = 24107;
        }

        internal override void Encode()
        {
            this.Enemy.Tick();
            using (Objects Home = new Objects(Enemy, Enemy.JSON))
            {
                this.Data.AddInt((int)(Home.Timestamp - DateTime.UtcNow).TotalSeconds);
                this.Data.AddInt(-1);

                this.Data.AddRange(Home.ToBytes);
                this.Data.AddRange(Enemy.Avatar.ToBytes);
                this.Data.AddRange(this.Device.Player.Avatar.ToBytes);

                this.Data.AddInt((int)this.BattleMode);
                this.Data.AddInt(0);
                this.Data.Add(0);
            }
        }

        internal override async void Process()
        {
            this.Device.Player.Avatar.Last_Attack_Enemy_ID.Add((int)this.Enemy.Avatar.UserId);

            if (this.Device.Player.Avatar.Last_Attack_Enemy_ID.Count > 20)
                this.Device.Player.Avatar.Last_Attack_Enemy_ID.RemoveAt(0);
            
            this.Device.State = Logic.Enums.State.IN_PC_BATTLE;

            if (this.Device.Player.Avatar.Battle_ID == 0)
            {
                Core.Resources.Battles.New(this.Device.Player, Core.Resources.Players.Get(this.Device.Player.Avatar.Last_Attack_Enemy_ID[this.Device.Player.Avatar.Last_Attack_Enemy_ID.Count - 1], Constants.Database), Constants.Database);
            }
            else
            {
                await Core.Resources.Battles.Save(Core.Resources.Battles.Get(this.Device.Player.Avatar.Battle_ID, Constants.Database, false));
                Core.Resources.Battles.Remove(this.Device.Player.Avatar.Battle_ID);

                Core.Resources.Battles.New(this.Device.Player, Core.Resources.Players.Get(this.Device.Player.Avatar.Last_Attack_Enemy_ID[this.Device.Player.Avatar.Last_Attack_Enemy_ID.Count - 1], Constants.Database), Constants.Database);
            }
        }

    }
}
