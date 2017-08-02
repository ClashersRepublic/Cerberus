using System;
using System.Threading;
using CRepublic.Magic.Core.Interface;
using CRepublic.Magic.Core.Network;
using CRepublic.Magic.Core.Resource;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Packets;

namespace CRepublic.Magic
{
    internal class Program
    {
        internal static int OP = 0;

        public static void Main(string[] args)
        {
            NativeCalls.SetWindowLong(NativeCalls.GetConsoleWindow(), -20, (int)NativeCalls.GetWindowLong(NativeCalls.GetConsoleWindow(), -20) ^ 0x80000);
            NativeCalls.SetLayeredWindowAttributes(NativeCalls.GetConsoleWindow(), 0, 217, 0x2);

            Control.Hi();
            Devices.Initialize();
            Gateway.Initialize();
            Gateway.Listen();

            Control.Say(@"-------------------------------------" + Environment.NewLine);

            Thread.Sleep(Timeout.Infinite);
        }

        internal static void TitleAdd()
        {
            Console.Title = Constants.Title + Interlocked.Increment(ref OP);
        }

        internal static void TitleDec()
        {
            Console.Title = Constants.Title + Interlocked.Decrement(ref OP);
        }
    }
}
