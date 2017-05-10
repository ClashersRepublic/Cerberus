using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BL.Proxy.Lyra
{
    struct Constants
    {
        public const int MF_BYCOMMAND = 0x00000000;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;
    }

    class NativeCalls
    {
        // IsDebuggerPresent()
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = false)]
        public static extern bool IsDebuggerPresent();

        // GetConsoleWindow()
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        // ShowWindow()
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // DeleteWindow()
        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        // GetSystemMenu()
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        /// <summary>
        /// Disables console resizing
        /// </summary>
        public static void DisableMenus()
        {
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                DeleteMenu(sysMenu, Constants.SC_MAXIMIZE, Constants.MF_BYCOMMAND);
                DeleteMenu(sysMenu, Constants.SC_SIZE, Constants.MF_BYCOMMAND);
            }
        }
    }
}
