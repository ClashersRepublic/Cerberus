using System;
using BL.Servers.CR.Extensions;
using BL.Servers.CR.Logic.Enums;
using BL.Servers.CR.Packets;
using System.Diagnostics;

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

                if (Message.Device.Connected())
                {
                    Debug.WriteLine("[MESSAGE] " + Message.Device.Socket.RemoteEndPoint.ToString() + " <-- " +  Message.GetType().Name);
                }

                Message.Process();
            }
            catch (Exception Exception)
            {
                Resources.Exceptions.Catch(Exception);
            }
        }

        internal static Command Handle(this Command Command)
        {
            Command.Encode();

            return Command;
        }
    }
}