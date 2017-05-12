using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Pluto
{
    internal static class Program
    {
        internal static string Hash(this string input)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
        }

        internal static string Hash(BufferedStream input)
        {
            var hash = new SHA1Managed().ComputeHash(input);
            return string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
        }

        internal static void Main(string[] args)
        {
            Console.WriteLine("Please enter your desired hash version");
            string Version = Console.ReadLine();

            var str = "{\"files\":[";

            var _files = Directory.GetFiles(args[0], args[3], SearchOption.AllDirectories);
            foreach (var path in _files)
            {
                var directoryInfo = new DirectoryInfo(Path.GetDirectoryName(path));
                using (var fileStream = new FileStream(path, FileMode.Open))
                {
                    using (var stream = new BufferedStream(fileStream))
                    {
                        str += "{\"sha\":\"" + Hash(stream) + "\",\"file\":\"" +
                               Path.Combine(directoryInfo.Name, Path.GetFileName(path)).Replace("\\", "\\/") + "\"},";
                        stream.Dispose();
                    }
                    fileStream.Dispose();
                }
            }

            var dateTimehash =
                DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))
                    .TotalSeconds.ToString(CultureInfo.InvariantCulture)
                    .Hash();
            var str1 = str.TrimEnd(',') + "],\"sha\":\"" + dateTimehash + "\",\"version\":\"" + Version + "\"}";
            var textWriter =
                new StreamWriter(new FileStream(Path.Combine(args[2], "fingerprint.json"), FileMode.CreateNew),
                    new UTF8Encoding(false));
            textWriter.Write(str1);
            textWriter.Dispose();
            Directory.CreateDirectory(args[1]);
            var destDirName = Path.Combine(args[1], dateTimehash);
            Directory.Move(args[0], destDirName);
        }
    }
}