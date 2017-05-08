namespace BL.Servers.BB.Core
{
    using System;
    using System.IO;
    using Logic.Enums;
    using NLog;

    internal class Loggers : IDisposable
    {
        private static Logger _logger;

        public Loggers()
        {
            if (Directory.Exists("Logs"))
            {
                if (Directory.GetFiles("Logs").Length > 0)
                {
                    var files = Directory.GetFiles("Logs", "*.txt", SearchOption.TopDirectoryOnly);
                    var date = new FileInfo(files[0]).LastWriteTime.ToString("dd-MM-yyyy - HH.mm");

                    Directory.CreateDirectory("Logs/" + date);

                    foreach (var file in files)
                    {
                        var info = new FileInfo(file);

                        if (File.Exists("Logs/" + date + "/" + info.Name))
                        {
                            File.Delete("Logs/" + date + "/" + info.Name);
                        }

                        info.MoveTo("Logs/" + date + "/" + info.Name);
                    }
                }
            }
            _logger = LogManager.GetCurrentClassLogger();

            _logger.Info("Logger has been started.");
            _logger.Warn("Logger has been started.");
            _logger.Error("Logger has been started.");

        }
        public static void Log(string message = "OK.", bool show = false, Defcon defcon = Defcon.DEFAULT)
        {
            switch (defcon)
            {

                case Defcon.INFO:
                {
                    _logger.Info(message);
                    break;
                }

                case Defcon.WARN:
                {
                    _logger.Warn(message);
                    break;
                }


                case Defcon.ERROR:
                {
                    _logger.Error(message);
                    break;
                }

                default:
                {
                    _logger.Info(message);
                    break;
                }
            }
            if (show)
                Console.WriteLine(message);
        }
        public void Dispose()
        {
            _logger = null;
        }
    }
}
