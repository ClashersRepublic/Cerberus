using System;
using System.Collections.Generic;
using System.Threading;
using CRepublic.Magic.Logic;

namespace CRepublic.Magic.Core
{
    internal class Devices : Dictionary<IntPtr, Device>
    {
        internal object Gate = new object();
        internal Devices()
        {
            // Devices.
        }

        internal void Add(Device Device)
        {
            lock (Gate)
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

            if (this.ContainsKey(Device.SocketHandle))
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