using System;
using System.Diagnostics;
using System.Threading;
using CRepublic.Royale.Core;
using CRepublic.Royale.Core.Interface;
using CRepublic.Royale.Core.Network;
using CRepublic.Royale.Core.Resource;
using CRepublic.Royale.Extensions;
using CRepublic.Royale.Files;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Packets;
using Newtonsoft.Json;

namespace CRepublic.Royale
{
    internal class Program
    {
        internal static int OP = 0;
        internal static Stopwatch Stopwatch { get; set; }
        internal static Random Random { get; set; }

        public static void Main(string[] args)
        {
            Random = new Random();
            Stopwatch = Stopwatch.StartNew();
            NativeCalls.SetWindowLong(NativeCalls.GetConsoleWindow(), -20, (int)NativeCalls.GetWindowLong(NativeCalls.GetConsoleWindow(), -20) ^ 0x80000);
            NativeCalls.SetLayeredWindowAttributes(NativeCalls.GetConsoleWindow(), 0, 217, 0x2);

            Control.Hi();
            Loggers.Initialize();
            Exceptions.Initialize();
            Devices.Initialize();
            Players.Initialize();
            Classes.Initialize();
            Gateway.Initialize();
            Gateway.Listen();

            Control.Say(@"-------------------------------------" + Environment.NewLine);
            
            while (true)
            {
                const int SLEEP_TIME = 5000;              

                Control.SayInfo("-- Pools --");
                Control.SayInfo($"SocketAsyncEventArgs: created -> {Gateway.NumberOfArgsCreated} in-use -> {Gateway.NumberOfArgsInUse} available -> {Gateway.NumberOfArgs}");
                Control.SayInfo($"Buffers: created -> {Gateway.NumberOfBuffersCreated} in-use -> {Gateway.NumberOfBuffersInUse} pool -> {Gateway.NumberOfBuffers} available -> {Gateway.NumberOfBuffers}");
                Thread.Sleep(SLEEP_TIME);
            }
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
