using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using CRepublic.Magic.Logic;

namespace CRepublic.Magic.Core.Resource
{
    internal static class Devices
    {
        internal static ConcurrentDictionary<IntPtr, Device> _Devices;

        internal static void Initialize()
        {
            _Devices = new ConcurrentDictionary<IntPtr, Device>();
        }

        internal static void Add(Device device)
        {
            _Devices.TryAdd(device.SocketHandle, device);
            Program.TitleAdd();
        }

        internal static bool Remove(IntPtr Socket)
        {
            var closedSocket = false;
            try
            {
                var device = default(Device);
                if (_Devices.TryRemove(Socket, out device))
                {
                    Program.TitleDec();

                    var socket = device.Socket;

                    try { socket.Shutdown(SocketShutdown.Both); }
                    catch { /* Swallow */ }
                    try { socket.Dispose(); }
                    catch { /* SWallow */ }

                    closedSocket = true;             
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
