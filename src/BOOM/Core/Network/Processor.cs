using System;
using CRepublic.Boom.Extensions;
using CRepublic.Boom.Packets;

namespace CRepublic.Boom.Core.Network
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
                Message.Encrypt();
                Resources.Gateway.Send(Message);
#if DEBUG
                if (Message.Device.Connected())
                {
                    Console.WriteLine(Utils.Padding(Message.Device.Socket.RemoteEndPoint.ToString(), 15) + " <-- " +  Message.GetType().Name);
                }
#endif

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