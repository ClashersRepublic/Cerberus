using System;
using CRepublic.Magic.Extensions.List;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Structure.Slots;

namespace CRepublic.Magic.Packets.Messages.Server.Battle
{
    internal class V2_Battle_Result : Message
    {
        internal Battle_V2 Battle;
        internal Logic.Structure.Slots.Items.Battle_V2 Home;
        internal Logic.Structure.Slots.Items.Battle_V2 Enemy;
        public V2_Battle_Result(Device Device, Battle_V2 Battle) : base(Device)
        {
            this.Identifier = 24371;
            this.Battle = Battle;
            this.Home = this.Battle.GetPlayerBattle(this.Device.Player.Avatar.UserId);
            this.Enemy = this.Battle.GetEnemyBattle(this.Device.Player.Avatar.UserId);
        }

        internal override void Encode()
        {
            this.Data.AddHexa("00 00 00 01 00 00 00 00 00 04 36 9F BE");

            this.Data.AddBool(this.Enemy.Attacker.ClanId > 0);
            if (this.Enemy.Attacker.ClanId > 0)
            {
                this.Data.AddLong(this.Enemy.Attacker.ClanId);
                this.Data.AddString(this.Enemy.Attacker.Alliance_Name);
                this.Data.AddInt(this.Enemy.Attacker.Badge_ID);
                this.Data.AddInt(this.Enemy.Attacker.Alliance_Level);
            }
            this.Data.AddLong(this.Enemy.Attacker.UserId);
            this.Data.AddLong(this.Enemy.Attacker.UserId);
            this.Data.AddString(this.Enemy.Attacker.Name);
            this.Data.AddInt(2);
            this.Data.AddByte(0);
            this.Data.AddVInt(15000);
            this.Data.AddVInt(15000);
            this.Data.AddVInt(15000);
            this.Data.AddVInt(15000);
            this.Data.AddVInt(0);
            this.Data.AddVInt((int)this.Enemy.Battle_Tick); //Opponent time left
            this.Data.AddBool(this.Enemy.Finished && (this.Home.Replay_Info.Stats.Destruction_Percentage > this.Enemy.Replay_Info.Stats.Destruction_Percentage));

            this.Data.AddInt(this.Home.Replay_Info.Stats.Attacker_Stars);
            this.Data.AddInt(this.Home.Replay_Info.Stats.Destruction_Percentage); //Home percentage

            this.Data.AddInt(this.Enemy.Finished ? this.Enemy.Replay_Info.Stats.Attacker_Stars : 0);
            this.Data.AddInt(this.Enemy.Finished ? this.Enemy.Replay_Info.Stats.Destruction_Percentage : 0); //Enemy percentage
            this.Data.AddInt(0); //Win or lost?
            this.Data.AddInt(0); //Win or lost?
            this.Data.AddInt(0); //Win or lost trophies
            this.Data.AddByte(this.Enemy.Finished ? 3 : 2);
            this.Data.AddInt(2); //Replay Low ID
            this.Data.AddInt(0);
            this.Data.AddInt(0); //Replay High ID
            this.Data.AddInt(8); //Major
            this.Data.AddInt(24);//Minor
            this.Data.AddInt(5); //Build

            this.Data.AddBool(this.Enemy.Finished);
            if (this.Enemy.Finished)
            {
                this.Data.AddInt(1); //Replay Low ID
                this.Data.AddInt(0);
                this.Data.AddInt(0); //Replay High ID
                this.Data.AddInt(8); //Major
                this.Data.AddInt(24); //Minor
                this.Data.AddInt(5); //Build
            }
            this.Data.AddInt(31);

            this.Data.AddString(this.Home.Replay_Info.Json);
            this.Data.AddString(this.Enemy.Finished ? this.Enemy.Replay_Info.Json : null);
        }
    }
}
