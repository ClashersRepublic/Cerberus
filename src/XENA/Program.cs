using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BB.Library.Patcher
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            Console.Title = $"BarbarianLand Library Patcher - ©BarbarianLand ";

            DirectoryInfo _folder = Directory.CreateDirectory("Patched");
            string[] _paths = Directory.GetFiles("Files", "*lib*", SearchOption.TopDirectoryOnly);

            foreach (string Path in _paths)
            {
                byte[] _file = File.ReadAllBytes(Path);

                if (Encoding.UTF8.GetString(_file, 0, _file.Length).Contains("clashofclans"))
                {
                    using (MemoryStream Stream = new MemoryStream(_file))
                    {
                        Console.WriteLine("- CoC : ");

                        long Offset = FindPosition(Stream, Constants.ClashOfClansAndroid);
                        if (Offset > -1)
                        {
                            Console.WriteLine("    - Offest       : " + (Offset - 32) + " [0x" + (Offset - 32).ToString("X") + "]");
                            Console.WriteLine("    - Key (Hex)    : " + "0x" + BitConverter.ToString(_file.ToList().GetRange((int)Offset - 32, 32).ToArray()).Replace("-", ", 0x"));
                            Console.WriteLine("    - Replace?");
                            Console.Write("\b");

                            if (Console.ReadKey(false).Key == ConsoleKey.Y)
                            {
                                List<Byte> Patched = _file.ToList();
                                Patched.RemoveRange((int) Offset - 32, 32);
                                Patched.InsertRange((int) Offset - 32, Constants.CustomKey);

                                using (FileStream FStream = File.Create(_folder.FullName + "/CoC-libg.so", Patched.Count, FileOptions.None))
                                {
                                    FStream.Write(Patched.ToArray(), 0, Patched.Count);
                                }
                            }
                        }
                        else
                        {
                            using (MemoryStream Stream2 = new MemoryStream(_file))
                            {
                                Offset = FindPosition(Stream2, Constants.OriginalKeyCoC);
                                if (Offset > -1)
                                {

                                    Console.WriteLine("    - Offest       : " + (Offset + 32) + " [0x" + (Offset + 32).ToString("X") + "]");
                                    Console.WriteLine("    - 20 byte sequence (Hex)    : " + "0x" + BitConverter.ToString(_file.ToList().GetRange((int)Offset + 32, 20).ToArray()).Replace("-", ", 0x"));
                                    Console.Write("\b");
                                    Console.ReadKey(false);
                                }
                            }
                        }
                    }
                }
                else if (Encoding.UTF8.GetString(_file, 0, _file.Length).Contains("clashroyale"))
                {
                    //May not work
                    using (MemoryStream Stream = new MemoryStream(_file))
                    {
                        long Offset = FindPosition(Stream, Constants.ClashRoyale);
                        Console.WriteLine("- CR : ");

                        if (Offset > -1)
                        {
                            Console.WriteLine("    - Offest       : " + (Offset - 32) + " [0x" + (Offset - 32).ToString("X") + "]");
                            Console.WriteLine("    - Key (Hex)    : " + "0x" + BitConverter.ToString(_file.ToList().GetRange((int)Offset - 32, 32).ToArray()).Replace("-", ", 0x"));
                            Console.WriteLine("    - Replace?");
                            Console.Write("\b");

                        }
                        else
                        {
                            using (MemoryStream Stream2 = new MemoryStream(_file))
                            {
                                Offset = FindPosition(Stream2, Constants.OriginalKeyCR);
                                if (Offset > -1)
                                {

                                    Console.WriteLine("    - Offest       : " + (Offset + 32) + " [0x" + (Offset + 32).ToString("X") + "]");
                                    Console.WriteLine("    - 20 byte sequence (Hex)    : " + "0x" + BitConverter.ToString(_file.ToList().GetRange((int)Offset + 32, 20).ToArray()).Replace("-", ", 0x"));
                                    Console.Write("\b");
                                    Console.ReadKey(false);
                                }
                            }
                        }
                        if (Console.ReadKey(false).Key == ConsoleKey.Y)
                        {
                            List<Byte> Patched = _file.ToList();
                            Patched.RemoveRange((int) Offset - 32, 32);
                            Patched.InsertRange((int) Offset - 32, Constants.CustomKey);

                            using (FileStream FStream = File.Create(_folder.FullName + "/CR-libg.so", Patched.Count, FileOptions.None))
                            {
                                FStream.Write(Patched.ToArray(), 0, Patched.Count);
                            }
                        }
                    }
                }
                else if (Encoding.UTF8.GetString(_file, 0, _file.Length).Contains("boombeach"))
                {
                    //May not work
                    using (MemoryStream Stream = new MemoryStream(_file))
                    {
                        long Offset = FindPosition(Stream, Constants.BoomBeach);
                        Console.WriteLine("- BB : ");

                        if (Offset > -1)
                        {
                            Console.WriteLine("    - Offest       : " + (Offset - 32) + " [0x" + (Offset - 32).ToString("X") + "]");
                            Console.WriteLine("    - Key (Hex)    : " + "0x" + BitConverter.ToString(_file.ToList().GetRange((int)Offset - 32, 32).ToArray()).Replace("-", ", 0x"));
                            Console.WriteLine("    - Replace?");
                            Console.Write("\b");

                        }
                        else
                        {
                            using (MemoryStream Stream2 = new MemoryStream(_file))
                            {
                                Offset = FindPosition(Stream2, Constants.OriginalKeyBB);
                                if (Offset > -1)
                                {

                                    Console.WriteLine("    - Offest       : " + (Offset + 32) + " [0x" + (Offset + 32).ToString("X") + "]");
                                    Console.WriteLine("    - 20 byte sequence (Hex)    : " + "0x" + BitConverter.ToString(_file.ToList().GetRange((int)Offset + 32, 20).ToArray()).Replace("-", ", 0x"));
                                    Console.Write("\b");
                                    Console.ReadKey(false);
                                }
                            }
                        }
                        if (Console.ReadKey(false).Key == ConsoleKey.Y)
                        {
                            List<Byte> Patched = _file.ToList();
                            Patched.RemoveRange((int)Offset - 32, 32);
                            Patched.InsertRange((int)Offset - 32, Constants.CustomKey);

                            using (FileStream FStream = File.Create(_folder.FullName + "/BB-libg.so", Patched.Count, FileOptions.None))
                            {
                                FStream.Write(Patched.ToArray(), 0, Patched.Count);
                            }
                        }
                    }
                }
            }
        }

        internal static long FindPosition(Stream _Stream, byte[] _Search)
        {
            byte[] Buffer = new byte[_Search.Length];

            using (BufferedStream Stream = new BufferedStream(_Stream, _Search.Length))
            {
                int Index = 0;
                while ((Index = Stream.Read(Buffer, 0, _Search.Length)) == _Search.Length)
                {
                    if (_Search.SequenceEqual(Buffer))
                    {
                        return Stream.Position - _Search.Length;
                    }
                    else
                    {
                        Stream.Position -= _Search.Length - Padding(Buffer, _Search);
                    }
                }
            }
            return -1;
        }

        internal static int Padding(byte[] _Bytes, byte[] _Search)
        {
            int Index = 1;
            {
                while (Index < _Bytes.Length)
                {
                    int Lenght = _Bytes.Length - Index;

                    byte[] Buffer1 = new byte[Lenght];
                    byte[] Buffer2 = new byte[Lenght];

                    Array.Copy(_Bytes, Index, Buffer1, 0, Lenght);
                    Array.Copy(_Search, Buffer2, Lenght);

                    if (Buffer1.SequenceEqual(Buffer2))
                    {
                        return Index;
                    }
                    Index++;
                }
                return Index;
            }
        }
    }
}