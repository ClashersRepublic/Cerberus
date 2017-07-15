using System.Linq;
using System.Collections.Generic;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Structure.Slots.Items;

namespace CRepublic.Magic.Core
{
    internal class Global_Chat : List<List_Devices>
    {
        internal const int Max_Devices_In_Chat = 200;
        internal const int Max_Chat = 1000;

        internal void Add(Device Device)
        {
            if (this.Count > 0)
            {
                if (this.OrderBy(C => C.Devices.Count).ToList()[0].Devices.Count >= Max_Devices_In_Chat &&
                    this.Count < Max_Chat)
                {
                    this.Add(new List_Devices(Device));
                }
                else
                {
                    this.OrderBy(C => C.Devices.Count).ToList()[0].Devices.Add(Device);
                }
            }
            else
                this.Add(new List_Devices(Device));
        }

        internal void Remove(Device Device)
        {
            int Index = this.FindIndex(C => C.Devices.ContainsKey(Device.Socket.Handle));

            if (Index > -1)
            {
                this[Index].Remove(Device);

                //if (this[Index].Devices.Count < 1)
                  //  this.RemoveAt(Index);
            }
        }

        internal Devices Get_Chat(Device Device)
        {
            int Index = this.FindIndex(C => C.Devices.ContainsKey(Device.Socket.Handle));

            if (Index > -1)
                return this[Index].Devices;
            this.Add(Device);
            return this.Get_Chat(Device);
        }
    }
}