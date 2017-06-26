using System;
using System.Collections.Concurrent;
using System.Threading;
using CRepublic.Magic.Logic;

namespace CRepublic.Magic.Core
{
    internal class Devices : ConcurrentDictionary<IntPtr, Device>
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
                    this.TryAdd(Device.SocketHandle, Device);
                }
            }
        }

        internal void Remove(IntPtr Socket)
        {
            try
            {
                var device = default(Device);
                if (this.TryRemove(Socket, out device))
                {
                    try
                    {
                        device.Socket.Disconnect(false);
                    }
                    catch
                    {
                        /* Swallow */
                    }
                    try
                    {
                        device.Socket.Close(5);
                    }
                    catch
                    {
                        /* Swallow */
                    }
                    try
                    {
                        device.Socket.Dispose();
                    }
                    catch
                    {
                        /* Swallow */
                    }

                    Interlocked.CompareExchange(ref device.Dropped, 1, 0);

                    if (device.Player != null)
                    {
                        if (Resources.Players.ContainsKey(device.Player.Avatar.UserId))
                        {
                            Resources.Players.Remove(device.Player);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Resources.Exceptions.Catch(ex, "Exception while dropping client.");
            }
        }
    }
}