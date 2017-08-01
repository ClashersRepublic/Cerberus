using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network;

namespace Magic.Packets.Commands.Client
{
	internal class TrainQuickUnitsCommand : Command
	{
        public int DataSlotID;
        public int Tick;

        public TrainQuickUnitsCommand(PacketReader br)
		{
			DataSlotID = br.ReadInt32();
			Tick = br.ReadInt32();
		}

		public override void Execute(Level level)
		{
			Avatar avatar = level.Avatar;
            if (DataSlotID == 1)
			{
				foreach (DataSlot dataSlot1 in avatar.QuickTrain1)
				{
                    DataSlot i = dataSlot1;
                    List<DataSlot> units = avatar.GetUnits();
                    DataSlot dataSlot2 = units.Find((Predicate<DataSlot>)(t => t.Data.GetGlobalID() == i.Data.GetGlobalID()));
                    if (dataSlot2 != null)
                    {
                        dataSlot2.Value = dataSlot2.Value + i.Value;
                    }
                    else
                    {
                        DataSlot dataSlot3 = new DataSlot(i.Data, i.Value);
                        units.Add(dataSlot3);
                    }
                }
			}
			else if (DataSlotID == 2)
			{
				foreach (DataSlot dataSlot1 in avatar.QuickTrain2)
				{
                    DataSlot i = dataSlot1;
                    List<DataSlot> units = avatar.GetUnits();
                    DataSlot dataSlot2 = units.Find((Predicate<DataSlot>)(t => t.Data.GetGlobalID() == i.Data.GetGlobalID()));
                    if (dataSlot2 != null)
                    {
                        dataSlot2.Value = dataSlot2.Value + i.Value;
                    }
                    else
                    {
                        DataSlot dataSlot3 = new DataSlot(i.Data, i.Value);
                        units.Add(dataSlot3);
                    }
                }
			}
			else
            {
              if (DataSlotID == 3)
                return;
				foreach (DataSlot dataSlot1 in avatar.QuickTrain3)
                {
                    DataSlot i = dataSlot1;
                    List<DataSlot> units = avatar.GetUnits();
                    DataSlot dataSlot2 = units.Find((Predicate<DataSlot>)(t => t.Data.GetGlobalID() == i.Data.GetGlobalID()));
                    if (dataSlot2 != null)
                    {
                        dataSlot2.Value = dataSlot2.Value + i.Value;
                    }
                    else
                    {
                        DataSlot dataSlot3 = new DataSlot(i.Data, i.Value);
                        units.Add(dataSlot3);
                    }
                }
			}			
		}
	}
}
