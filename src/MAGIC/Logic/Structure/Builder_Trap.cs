using CRepublic.Magic.Files.CSV_Helpers;
using CRepublic.Magic.Files.CSV_Logic;
using CRepublic.Magic.Logic.Components;

namespace CRepublic.Magic.Logic.Structure
{
    internal class Builder_Trap : ConstructionItem
    {
        public Builder_Trap(Data data, Level l) : base(data, l)
        {
            AddComponent(new Trigger_Component());
        }

        internal override int ClassId => 11;

        public Traps GetTrapData => (Traps)GetData();
    }
}