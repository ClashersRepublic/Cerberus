using Magic.ClashOfClans.Core.Settings;
using System;
using System.IO;

namespace Magic.ClashOfClans.Core
{
    public static class ExceptionLogger
    {
        private static readonly object s_sync = new object();

        public static void Initialize()
        {
            if (!Directory.Exists("logs"))
                Directory.CreateDirectory("logs");
        }

        public static void Log(Exception ex, string moreInfo)
        {
            var type = ex.GetType();
            var name = type.Name;
            var dirPath = Path.Combine("logs", name);

            if (!Directory.Exists(name))
                Directory.CreateDirectory(dirPath);

            var text = moreInfo + ": " + ex.ToString() + "\r\n\r\n";
            var filePath = Path.Combine(dirPath, GetLogFileName(ex));

            lock (s_sync)
                File.AppendAllText(filePath, text);

            if (Constants.Verbosity > 2)
                Logger.Error(moreInfo + ": " + ex.GetType().Name + ": " + ex.Message);
        }

        private static string GetLogFileName(Exception ex)
        {
            if (string.IsNullOrWhiteSpace(ex.Message))
                return "unknown.log";
            else
                return StripString(ex.Message).Replace(".", "") + ".log";
        }

        private static string StripString(string value)
        {
            // Trim the string.
            if (value.Length > 50)
                value = value.Substring(0, 50);

            var invalidChars = Path.GetInvalidFileNameChars();
            var final = string.Empty;
            for (int i = 0; i < value.Length; i++)
            {
                var invalid = false;
                for (int j = 0; j < invalidChars.Length; j++)
                {
                    if (value[i] == invalidChars[j])
                    {
                        invalid = true;
                        break;
                    }
                }

                if (!invalid)
                    final += value[i];
            }
            return final;
        }
    }
}
