namespace BL.Servers.CR.Core
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using BL.Servers.CR.Logic;

    internal class Devices : Dictionary<IntPtr, Device>
    {
        internal Devices()
        {
            // Devices.
        }
        
        internal void Add(Device Device)
        {
            if (this.ContainsKey(Device.SocketHandle))
            {
                this[Device.SocketHandle] = Device;
            }
            else
            {
                this.Add(Device.SocketHandle, Device);
            }
        }
        
        internal void Remove(Device Device)
        {
            if (Device.Player != null)
            {
                if (Resources.Players.ContainsValue(Device.Player))
                {
                    if (Resources.Players.ContainsValue(Device.Player))
                    {
                        Resources.Players.Remove(Device.Player);
                    }
                }
            }
            else if (this.ContainsKey(Device.SocketHandle))
            {
                this.Remove(Device.SocketHandle);

                try
                {
                    Device.Socket.Disconnect(false);
                }
                catch (Exception)
                {
                    // Already Closed.
                }

                try
                {
                    Device.Socket.Close(5);
                }
                catch (Exception)
                {
                    // Already Closed.
                }

                try
                {
                    Device.Socket.Dispose();
                }
                catch (Exception)
                {
                    // Already Closed.
                }
            }
        }
    }
}