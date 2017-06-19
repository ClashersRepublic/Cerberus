namespace CRepublic.Boom.Logic.Structure
{
    using CRepublic.Boom.Files.CSV_Helpers;
    using CRepublic.Boom.Files.CSV_Logic;
    using CRepublic.Boom.Logic.Components;
    internal class Trap : ConstructionItem
    {
        public Trap(Data data, Level l) : base(data, l)
        {
            AddComponent(new Trigger_Component());
        }

        internal override int ClassId => 4;

        public Traps GetTrapData() => (Traps)GetData();
    }
}