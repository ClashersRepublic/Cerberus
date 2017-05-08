using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Packets;

namespace BL.Servers.CoC.Core.Networking
{
    internal static class Processor
    {
        internal static void Recept(this Message Message)
        {
            Message.Decrypt();
            Message.Decode();
            Message.Process();
        }

        internal static void Send(this Message Message)
        {
            try
            {

                Message.Encode();

#if DEBUG
                if (Message.Device.Connected())
                {
                    Console.WriteLine(Utils.Padding(Message.Device.Socket.RemoteEndPoint.ToString(), 15) + " <-- " +  Message.GetType().Name);
                    Loggers.Log(Message, Utils.Padding(Message.Device.Socket.RemoteEndPoint.ToString(), 15));

                }
#endif
                Message.Encrypt();
                Resources.Gateway.Send(Message);

                Message.Process();
            }
            catch (Exception)
            {
            }
        }

        internal static Command Handle(this Command Command)
        {
            Command.Encode();

            return Command;
        }
    }
}