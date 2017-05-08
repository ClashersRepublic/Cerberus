using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Packets;
using NLog;

namespace BL.Servers.CoC.Core
{
    internal class Loggers : IDisposable
    {
        internal static Logger _logger;

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
            _logger.Debug("Logger has been started.");

        }
        internal static void Log(string message = "OK.", bool show = false, Defcon defcon = Defcon.DEFAULT)
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

        internal static void Log(Message Message, string prefix = null)
        {
            StringBuilder packet = new StringBuilder();
            packet.Append($"{DateTime.Now:yyyy/MM/dd/HH/mm/ss};");
            if (!string.IsNullOrEmpty(prefix))
            {
                packet.Append($"{prefix};");
            }
            packet.Append($"{Message.Identifier}({Message.Version});{Message.Length};");
            if (Message.Data != null)
            {
                packet.AppendLine(BitConverter.ToString(Message.Data.ToArray()).Replace("-", String.Empty));
                packet.AppendLine(Regex.Replace(Encoding.UTF8.GetString(Message.Data.ToArray()),
                    @"[^\u0020-\u007F]", "."));
            }
            _logger.Debug(packet.ToString);
        }

        public void Dispose()
        {
            _logger = null;
        }
    }
}
