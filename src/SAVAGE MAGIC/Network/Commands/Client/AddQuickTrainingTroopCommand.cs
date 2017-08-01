using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Magic.ClashOfClans.Core;
using Magic.Files.Logic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class AddQuicKTrainingTroopCommand : Command
    {
        public int Database;
        public int Tick;
        public int TroopType;

        public List<UnitToAdd> UnitsToAdd { get; set; }

        public AddQuicKTrainingTroopCommand(PacketReader br)
        {
            UnitsToAdd = new List<UnitToAdd>();

            Database = br.ReadInt32();
            TroopType = br.ReadInt32();

            for (int i = 0; i < TroopType; i++)
            {
                UnitsToAdd.Add(new UnitToAdd()
                {
                    Data = (CombatItemData)br.ReadDataReference(),
                    Count = br.ReadInt32()
                });
            }
            Tick = br.ReadInt32();
        }

        public override void Execute(Level level)
        {
            var defaultDatabase = level.Avatar.QuickTrain1;
            if (Database == 1)
            {
                defaultDatabase.Clear();
            }
            else if (Database == 2)
            {
                defaultDatabase = level.Avatar.QuickTrain2;
                defaultDatabase.Clear();
            }
            else if (Database == 3)
            {
                defaultDatabase = level.Avatar.QuickTrain3;
                defaultDatabase.Clear();
            }
            else throw new InvalidDataException("Unknown database in AddQuickTrainingTroopCommand.");

            foreach (var i in UnitsToAdd)
            {
                DataSlot ds = new DataSlot(i.Data, i.Count);
                defaultDatabase.Add(ds);
            }
        }

        internal class UnitToAdd
        {
            public int Count { get; set; }

            public CombatItemData Data { get; set; }
        }
    }
}
