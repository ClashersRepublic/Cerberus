using System;
using BL.Servers.CR.Extensions;
using BL.Servers.CR.Logic.Enums;
using BL.Servers.CR.Packets;
using System.Diagnostics;

namespace BL.Servers.CR.Core.Network
{
    internal static class Processor
    {
        internal static void Recept(this Packets.Message Message)
        {
            if (Constants.Encryption == Logic.Enums.Server_Crypto.RC4)
            {
                Message.DecryptRC4();
            }
            else
            {
                Message.DecryptSodium();
            }
            
            Message.Decode();
            
            Message.Process();
        }

        internal static void Send(this Packets.Message Message)
        {
            try
            {
                Message.Encode();

                if (Constants.Encryption == Logic.Enums.Server_Crypto.RC4)
                {
                    Message.EncryptRC4();
                }
                else
                {
                    Message.EncryptSodium();
                }
                
                Server_Resources.Gateway.Send(Message);

                if (Message.Device.Connected())
                {
                    Debug.WriteLine("[MESSAGE] " + Message.Device.Socket.RemoteEndPoint.ToString() + " <-- " +  Message.GetType().Name + " [" + Message.Identifier + "]");
                }

                Message.Process();              
            }
            catch (System.Exception Exception)
            {
                Console.WriteLine("Something when wrong! " + Exception);

                //Resources.Exceptions.Catch(Exception, ErrorLevel.Critical);
            }
        }

        internal static Command Handle(this Command Command)
        {
            Command.Encode();

            return Command;
        }
    }
}