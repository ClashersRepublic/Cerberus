using System;
using BL.Servers.CR.Extensions;
using BL.Servers.CR.Logic.Enums;
using BL.Servers.CR.Packets;

namespace BL.Servers.CR.Core.Network
{
    internal static class Processor
    {
        internal static void Recept(this Message Message)
        {
            if (Constants.Encryption == Logic.Enums.Crypto.RC4)
                Message.DecryptRC4();
            else
                Message.DecryptSodium();

            Message.Decode();
            Message.Process();
        }

        internal static void Send(this Message Message)
        {
            try
            {
                Message.Encode();

                if (Constants.Encryption == Logic.Enums.Crypto.RC4)
                    Message.EncryptRC4();
                else
                    Message.EncryptSodium();

                Resources.Gateway.Send(Message);
#if DEBUG
                if (Message.Device.Connected())
                {
                    Console.WriteLine(Utils.Padding(Message.Device.Socket.RemoteEndPoint.ToString(), 15) + " <-- " +  Message.GetType().Name);
                }
#endif

                Message.Process();
            }
            catch (Exception Exception)
            {
                Loggers.Log(Utils.Padding(Exception.GetType().Name, 15) + " : " + Exception.Message + Environment.NewLine + Exception.StackTrace, true, Defcon.ERROR);
            }
        }

        internal static Command Handle(this Command Command)
        {
            Command.Encode();

            return Command;
        }
    }
}