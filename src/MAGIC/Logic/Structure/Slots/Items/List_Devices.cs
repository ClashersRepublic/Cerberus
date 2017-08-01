using System;
using System.Collections.Concurrent;
using CRepublic.Magic.Core;
using CRepublic.Magic.Extensions;

namespace CRepublic.Magic.Logic.Structure.Slots.Items
{
    internal class List_Devices
    {
        internal ConcurrentDictionary<IntPtr, Device> Devices = new ConcurrentDictionary<IntPtr, Device>();

        internal List_Devices(Device Device)
        {
            Devices.TryAdd(Device.Socket.Handle, Device);
        }

        internal void Add(Device Device)
        {
            if (Devices.ContainsKey(Device.SocketHandle))
            {
                Devices[Device.SocketHandle] = Device;
            }
            else
            {
                Devices.TryAdd(Device.SocketHandle, Device);
            }
        }

        internal void Remove(Device Device)
        {
            if (Device != null)
            {
                if (Devices.ContainsKey(Device.Socket.Handle))
                {
                    Devices.TryRemove(Device.Socket.Handle);
                }
                //else
                    //this.Devices.Remove(Device.Socket.Handle);
            }
        }
    }
}