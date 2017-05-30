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
       /* internal static void Recept(this Message Message)
        {
            Message.Decryptpe();
            Message.Decode();
            Message.Process();
        }*/

        internal static void Send(this Message Message)
        {
            try
            {

                Message.Encode();

#if DEBUG
                Loggers.Log(Message, Utils.Padding(Message.Device.Socket.RemoteEndPoint.ToString(), 15));
#endif
                if (Constants.RC4)
                    Message.EncryptRC4();
                else
                    Message.EncryptPepper();

                Resources.Gateway.Send(Message);
#if DEBUG
                if (Message.Device.Connected())
                {
                    Loggers.Log(Utils.Padding(Message.Device.Socket.RemoteEndPoint.ToString(), 15) + " <-- " +
                                      Message.GetType().Name, true);
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