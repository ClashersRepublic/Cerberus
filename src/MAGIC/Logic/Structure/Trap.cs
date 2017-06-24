using CRepublic.Magic.Files.CSV_Helpers;
using CRepublic.Magic.Files.CSV_Logic;
using CRepublic.Magic.Logic.Components;

namespace CRepublic.Magic.Logic.Structure
{
    internal class Trap : ConstructionItem
    {
        public Trap(Data data, Level l) : base(data, l)
        {
            AddComponent(new Trigger_Component());
            if (GetTrapData.HasAltMode || GetTrapData.DirectionCount > 0)
            {
                AddComponent(new Combat_Component(this));
            }
        }

        internal override int ClassId => 4;

        public Traps GetTrapData => (Traps)GetData();
    }
}