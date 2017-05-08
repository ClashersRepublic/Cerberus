using System;
using System.IO;
using System.Text;

namespace BL.Servers.CoC.Extensions
{
    internal class Prefixed : TextWriter
    {
        internal readonly TextWriter Original;

        internal Prefixed()
        {
            this.Original = Console.Out;
        }

        public override Encoding Encoding => new ASCIIEncoding();

        public override void Write(string Message)
        {
            this.Original.Write("[BL.Servers.CoC]    {0}", Message);
        }

        public override void WriteLine(string Message)
        {
            if (Message.Length <= Console.WindowWidth)
            {
                Console.SetCursorPosition((Console.WindowWidth - Message.Length) / 2, Console.CursorTop);
            }

            this.Original.WriteLine("{0}", Message);
        }

        public override void WriteLine()
        {
            this.Original.WriteLine();
        }

    }
}