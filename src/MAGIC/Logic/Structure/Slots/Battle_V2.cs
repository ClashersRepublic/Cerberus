namespace CRepublic.Magic.Logic.Structure.Slots
{
    internal class Battle_V2
    {
        internal int BattleID;
        internal int Tick;
        internal int Checksum;
        internal bool Started;

        internal Level Player1;
        internal Level Player2;

        internal Items.Battle_V2 Battle1;
        internal Items.Battle_V2 Battle2;
        
        internal Timer Timer = new Timer();

        public Battle_V2()
        {

        }

        public Battle_V2(Level _Player1, Level _Player2)
        {
            this.Player1 = _Player1;
            this.Player2 = _Player2;
            this.Battle1 = new Items.Battle_V2(_Player1, _Player2);
            this.Battle2 = new Items.Battle_V2(_Player2, _Player1);

        }

        internal Level GetPlayer(long UserID)
        {
            return this.Battle1.Attacker.UserId == UserID ? this.Player1 : this.Player2;
        }

        internal Level GetEnemy(long UserID)
        {
            return this.Battle1.Attacker.UserId != UserID ? this.Player1 : this.Player2;
        }

        internal Items.Battle_V2 GetPlayerBattle(long UserID)
        {
            return this.Battle1.Attacker.UserId == UserID ? this.Battle1 : this.Battle2;
        }

        internal Items.Battle_V2 GetEnemyBattle(long UserID)
        {

            return this.Battle2.Attacker.UserId != UserID ? this.Battle2 : this.Battle1;
        }
    }
}
