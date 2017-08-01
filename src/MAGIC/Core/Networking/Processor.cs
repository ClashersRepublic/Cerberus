using System;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Packets;

namespace CRepublic.Magic.Core.Networking
{
    internal static class Processor
    {
        internal static void Recept(this Message Message)
        {
            if (Constants.RC4)
                Message.EncryptRC4();
            else
                Message.EncryptPepper();
            Message.Decode();
            Message.Process();
        }


        internal static void Send(this Message Message)
        {
            try
            {
                try
                {
                    Message.Encode();
                }
                catch (Exception ex)
                {
                    Exceptions.Log(ex, $"Exception while encoding message {Message.GetType()}");
                    return;
                }

#if DEBUG
                Loggers.Log(Message, Utils.Padding(Message.Device.Socket.RemoteEndPoint.ToString(), 15));
#endif
                if (Constants.RC4)
                    Message.EncryptRC4();
                else
                    Message.EncryptPepper();

                Gateway.Send(Message);
#if DEBUG
                if (Message.Device.Connected())
                {
                    Loggers.Log(Utils.Padding(Message.Device.Socket.RemoteEndPoint.ToString(), 15) + " <-- " + Message.GetType().Name, true);
                }
#endif
                Message.Process();
            }
            catch (Exception)
            {
                //
            }
        }

        internal static Command Handle(this Command Command)
        {
            Command.Encode();

            return Command;
        }
    }
}