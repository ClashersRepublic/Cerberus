using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BL.Proxy.Lyra
{
    class Helper
    {
        /// <summary>
        /// Creates required sub-directories
        /// </summary>
        public static void CheckDirectories()
        {
            if (!Directory.Exists("JsonDefinitions"))
                Directory.CreateDirectory("JsonDefinitions");
            if (!Directory.Exists("JsonPackets"))
                Directory.CreateDirectory("JsonPackets");
            if (!Directory.Exists("RawPackets"))
                Directory.CreateDirectory("RawPackets");
            if (!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");
        }

        /// <summary>
        /// Returns if the process is being debugged
        /// </summary>
        public static bool IsDebugging => Debugger.IsAttached || NativeCalls.IsDebuggerPresent();

        /// <summary>
        /// Returns the local network IP in a proper format
        /// </summary>
        public static IPAddress LocalNetworkIP
        {
            get
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP))
                {
                    socket.Connect("10.0.2.4", 65530);
                    return ((IPEndPoint)socket.LocalEndPoint).Address;
                }
            }
        }

        /// <summary>
        /// Returns a random console color
        /// </summary>
        public static ConsoleColor ChooseRandomColor()
        {
            var randomIndex = new Random().Next(0, Enum.GetNames(typeof(ConsoleColor)).Length);
            var color = (ConsoleColor)randomIndex;

            if (color != ConsoleColor.Black)
                return (ConsoleColor)randomIndex;
            else
                return ConsoleColor.Green;
        }

        /// <summary>
        /// Returns Proxy-Version in the following format:
        /// v1.2.3
        /// </summary>
        public static string AssemblyVersion
        {
            get
            {
                return "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Remove(5);
            }
        }

        /// <summary>
        /// Returns the actual time in a localized format
        /// </summary>
        public static string ActualTime
        {
            get
            {
                return DateTime.Now.ToString("h:mm:ss");
            }
        }

        /// <summary>
        /// Hides the cmd window
        /// </summary>
        public static void HideConsole()
        {
            NativeCalls.ShowWindow(NativeCalls.GetConsoleWindow(), 0);
           
        }   
    }
}
