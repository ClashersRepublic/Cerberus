using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;
using CRepublic.Magic.Logic;

namespace CRepublic.Magic.Core
{
    internal static class Devices
    {
        internal static ConcurrentDictionary<IntPtr, Device> _Devices;
        internal static void Initialize()
        {
            _Devices = new ConcurrentDictionary<IntPtr, Device>();
        }

        internal static void Add(Device Device)
        {
            _Devices.TryAdd(Device.SocketHandle, Device);
        }

        internal static bool Remove(IntPtr Socket)
        {
            var closedSocket = false;
            try
            {
                var device = default(Device);
                if (_Devices.TryRemove(Socket, out device))
                {

                    var socket = device.Socket;

                    try { socket.Shutdown(SocketShutdown.Both); }
                    catch { /* Swallow */ }
                    try { socket.Dispose(); }
                    catch { /* SWallow */ }

                    closedSocket = true;
                    if (device.Player != null)
                    {
                        if (Players.Levels.ContainsKey(device.Player.Avatar.UserId))
                        {
                            Players.Remove(device.Player);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Exceptions.Log(ex, "Exception while dropping client.");
            }
            return closedSocket;
        }
    }
}