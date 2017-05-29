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
            {
                Console.WriteLine("Decrypting RC4 message.");
                Message.DecryptRC4();
            }
            else
            {
                Console.WriteLine("Decrypting Sodium message.");
                Message.DecryptSodium();
            }

            Console.WriteLine("Decoding message.");
            Message.Decode();

            Console.WriteLine("Processing message.");
            Message.Process();

            Console.WriteLine("Successfully processed message!");
        }

        internal static void Send(this Message Message)
        {
            try
            {
                Console.WriteLine("Encoding message.");

                Message.Encode();

                if (Constants.Encryption == Logic.Enums.Crypto.RC4)
                {
                    Console.WriteLine("Encrypting RC4 message.");
                    Message.EncryptRC4();
                }
                else
                {
                    Console.WriteLine("Encrypting Sodium message.");
                    Message.EncryptSodium();
                }

                Console.WriteLine("Sending message.");
                Resources.Gateway.Send(Message);

                if (Message.Device.Connected())
                {
                    Debug.WriteLine("[MESSAGE] " + Message.Device.Socket.RemoteEndPoint.ToString() + " <-- " +  Message.GetType().Name);
                }

                Console.WriteLine("Processing message.");
                Message.Process();

                Console.WriteLine("Sucessfully processed message!");
            }
            catch (Exception Exception)
            {
                Resources.Exceptions.Catch(Exception, "There was an exception while encryting the message");
            }
        }

        internal static Command Handle(this Command Command)
        {
            Command.Encode();

            return Command;
        }
    }
}