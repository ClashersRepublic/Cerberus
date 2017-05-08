using System;
using BL.Servers.CR.Extensions;
using BL.Servers.CR.Packets;

namespace BL.Servers.CR.Core.Network
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

#if DEBUG
                if (Message.Device.Connected())
                {
                    Console.WriteLine(Utils.Padding(Message.Device.Socket.RemoteEndPoint.ToString(), 15) + " <-- " +
                                      Message.GetType().Name);
                }
#endif
                Message.Encode();
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