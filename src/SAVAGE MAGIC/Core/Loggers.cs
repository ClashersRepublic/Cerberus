using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Magic.Core.Interface;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Logic.Enums;
using Ionic.Zip;
using Ionic.Zlib;
using NLog;

namespace CRepublic.Magic.Core
{
    internal static class Loggers
    {
        internal static Logger _logger;

        internal static void Initialize()
        {
            if (Directory.Exists("Logs"))
            {
                if (Directory.GetFiles("Logs").Length > 0)
                {
                    Control.SayInfo("Compressing server previous 'logs'...");

                    if (Directory.Exists("Logs"))
                        Directory.CreateDirectory("Logs/Logs-Backup/");

                    var files = Directory.GetFiles("Logs", "*.txt", SearchOption.TopDirectoryOnly);
                    var date = new FileInfo(files[0]).LastWriteTime.ToString("dd-MM-yyyy - HH.mm.ss");

                    using (var zip = new ZipFile($"Logs/Logs-Backup/{date}.zip"))
                    {
                        foreach (var file in files)
                        {
                            var info = new FileInfo(file);
                            zip.AddFile(file);

                            if (File.Exists("Logs/Logs-Backup/" + date + "/" + info.Name))
                            {
                                File.Delete("Logs/Logs-Backup/" + date + "/" + info.Name);
                            }
                        }
                        zip.Save();
                    }
                }
            }
            else
                Directory.CreateDirectory("Logs");

            _logger = LogManager.GetCurrentClassLogger();

            _logger.Info("Logger has been started.");
            _logger.Warn("Logger has been started.");
            _logger.Error("Logger has been started.");
            _logger.Fatal("Logger has been started.");
            _logger.Trace("Logger has been started.");
            _logger.Debug("Logger has been started.");

        }

        internal static void Log(string message = "OK.", Defcon defcon = Defcon.DEFAULT)
        {
            if (Constants.WriteLog)
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

                    case Defcon.TRACE:
                    {
                        _logger.Trace(message);
                        break;
                    }

                    case Defcon.FATAL:
                    {
                        _logger.Fatal(message);
                        break;
                    }

                    default:
                    {
                        _logger.Info(message);
                        break;
                    }
                }
            }
        }
    }
}
