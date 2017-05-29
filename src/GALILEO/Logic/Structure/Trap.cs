using BL.Servers.CoC.Files.CSV_Helpers;
using BL.Servers.CoC.Files.CSV_Logic;
using BL.Servers.CoC.Logic.Components;

namespace BL.Servers.CoC.Logic.Structure
{
    internal class Trap : ConstructionItem
    {
        public Trap(Data data, Level l) : base(data, l)
        {
            AddComponent(new Trigger_Component());
            if (GetTrapData.HasAltMode)
            {
                AddComponent(new Combat_Component(this));
            }
        }

        internal override int ClassId => 4;

        public Traps GetTrapData => (Traps)GetData();
    }
}