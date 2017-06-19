using Republic.Magic.Files.CSV_Helpers;
using Republic.Magic.Files.CSV_Logic;
using Republic.Magic.Logic.Components;

namespace Republic.Magic.Logic.Structure
{
    internal class Builder_Trap : ConstructionItem
    {
        public Builder_Trap(Data data, Level l) : base(data, l)
        {
            AddComponent(new Trigger_Component());
            if (GetTrapData.HasAltMode)
            {
                AddComponent(new Combat_Component(this));
            }
        }

        internal override int ClassId => 11;

        public Traps GetTrapData => (Traps)GetData();
    }
}