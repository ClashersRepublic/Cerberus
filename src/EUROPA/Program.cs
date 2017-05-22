using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BL.Assets.Hasher
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
            Console.SetOut(new Prefixed());
            Console.WriteLine(@"__________             ___.                .__                     .____                       .___");
            Console.WriteLine(@"\______   \_____ ______\_ |__ _____ _______|__|____    ____   _____|    |   _____    ____    __| _/");
            Console.WriteLine(@" |    |  _/\__  \\_  __ \ __ \\__  \\_  __ \  \__  \  /    \ /  ___/    |   \__  \  /    \  / __ | ");
            Console.WriteLine(@" |    |   \ / __ \|  | \/ \_\ \/ __ \|  | \/  |/ __ \|   |  \\___ \|    |___ / __ \|   |  \/ /_/ | ");
            Console.WriteLine(@" |______  /(____  /__|  |___  (____  /__|  |__(____  /___|  /____  >_______ (____  /___|  /\____ | ");
            Console.WriteLine(@"        \/      \/          \/     \/              \/     \/     \/        \/    \/     \/      \/  ");
            Console.WriteLine(@"                                                                           Developer Edition  ");
            Console.WriteLine("Please enter your desired hash version");
            var Version = Console.ReadLine();

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

            var dateTimehash = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds
                .ToString(CultureInfo.InvariantCulture).Hash();
            var str1 = str.TrimEnd(',') + "],\"sha\":\"" + dateTimehash + "\",\"version\":\"" + Version + "\"}";
            Directory.CreateDirectory(args[1]);
            var destDirName = Path.Combine(args[1], dateTimehash);
            Directory.Move(args[0], destDirName);

            var textWriter =
                new StreamWriter(new FileStream(Path.Combine(destDirName, "fingerprint.json"), FileMode.CreateNew),
                    new UTF8Encoding(false));
            textWriter.Write(str1);
            textWriter.Dispose();

            if (File.Exists(Path.Combine(args[1], "VERSION")))
                File.Move(Path.Combine(args[1], "VERSION"), Path.Combine(args[1], "VERSION.Old"));
            var versionfile = new StringBuilder();
            versionfile.AppendLine(Version);
            versionfile.AppendLine(dateTimehash);
            

            var textWriter2 = new StreamWriter(new FileStream(Path.Combine(args[1], "VERSION"), FileMode.CreateNew),
                new UTF8Encoding(false));
            textWriter2.Write(versionfile.ToString());
            textWriter2.Dispose();
        }
    }
}