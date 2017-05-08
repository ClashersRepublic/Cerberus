namespace BL.Servers.BB.Logic.Structure
{
    using BL.Servers.BB.Files.CSV_Helpers;
    using BL.Servers.BB.Files.CSV_Logic;
    using BL.Servers.BB.Logic.Components;
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