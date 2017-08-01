using System;
using System.Diagnostics;
using System.IO;
using Magic.Files.Logic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class PlaceAttackerCommand : Command
    {
        public CombatItemData Unit;
        public uint Tick;
        public int X;
        public int Y;

        public PlaceAttackerCommand(PacketReader br)
        {
            X = br.ReadInt32();
            Y = br.ReadInt32();
            Unit = (CombatItemData)br.ReadDataReference();
            Tick = br.ReadUInt32();
        }

        public override void Execute(Level level)
        {
            level.Avatar.AddUsedTroop(Unit, 1);

            var avatar = level.Avatar;
            var units = avatar.GetUnits();
            for (int i = 0; i < units.Count; i++)
            {
                var unit = units[i];
                if (unit.Data.GetGlobalID() == Unit.GetGlobalID())
                    unit.Value -= 1;
            }

            //var components = level.GetComponentManager().GetComponents(0);
            //for (var i = 0; i < components.Count; i++)
            //{
            //    var c = (UnitStorageComponent)components[i];
            //    if (c.GetUnitTypeIndex(Unit) != -1)
            //    {
            //        var storageCount = c.GetUnitCountByData(Unit);
            //        if (storageCount >= 0)
            //        {
            //            c.RemoveUnits(Unit, 1);
            //            break;
            //        }
            //    }
            //}
        }
    }
}