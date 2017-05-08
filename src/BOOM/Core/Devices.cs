namespace BL.Servers.BB.Core
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using BL.Servers.BB.Logic;

    internal class Devices : Dictionary<IntPtr, Device>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Devices"/> class.
        /// </summary>
        internal Devices()
        {
            // Devices.
        }

        /// <summary>
        /// Adds the specified device.
        /// </summary>
        /// <param name="Device">The device.</param>
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

        /// <summary>
        /// Removes the specified device.
        /// </summary>
        /// <param name="Device">The device.</param>
        internal void Remove(Device Device)
        {
            if (Device.Player != null)
            {
                Resources.Players.Remove(Device.Player);
            }
            else
            {
                if (this.ContainsKey(Device.SocketHandle))
                {
                    this.Remove(Device.SocketHandle);
                }

                try
                {
                    Device.Socket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception)
                {
                    // Already Closed.
                }

                Device.Socket.Close(5);
            }

        }
    }
}