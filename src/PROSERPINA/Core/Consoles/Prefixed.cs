namespace BL.Servers.CR.Core.Consoles
{
    using System;
    using System.IO;
    using System.Text;

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
            this.Original.Write("[BL.Servers]    {0}", Message);
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