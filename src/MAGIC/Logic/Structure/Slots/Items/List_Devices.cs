using CRepublic.Magic.Core;
using CRepublic.Magic.Extensions;

namespace CRepublic.Magic.Logic.Structure.Slots.Items
{
    internal class List_Devices
    {
        internal Devices Devices = new Devices();

        public List_Devices(Device Device)
        {
            this.Devices.TryAdd(Device.Socket.Handle, Device);
        }

        internal void Remove(Device Device)
        {
            if (Device != null)
            {
                if (this.Devices.ContainsKey(Device.Socket.Handle))
                {
                    this.Devices.TryRemove(Device.Socket.Handle);
                }
                //else
                    //this.Devices.Remove(Device.Socket.Handle);
            }
        }
    }
}