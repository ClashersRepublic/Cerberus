namespace Magic.ClashOfClans.Logic
{
    internal class Achievement
    {
        private const int m_vType = 23000000;

        public int Id
        {
            get
            {
                return 23000000 + this.Index;
            }
        }

        public int Index { get; set; }

        public string Name { get; set; }

        public bool Unlocked { get; set; }

        public int Value { get; set; }

        public Achievement()
        {

        }

        public Achievement(int index)
        {
            Index = index;
            Unlocked = false;
            Value = 0;
        }
    }
}
