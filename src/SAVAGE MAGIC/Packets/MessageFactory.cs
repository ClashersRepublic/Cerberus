using System;
using System.Collections.Generic;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Packets.Messages.Client;
using CRepublic.Magic.Packets.Messages.Client.Authentication;

namespace CRepublic.Magic.Packets
{
    internal static class MessageFactory
    {
        public static Dictionary<int, Type> Messages;

         internal static void Initialize()
        {
            Messages = new Dictionary<int, Type>
            {
                {10100, typeof(Pre_Authentication) },
                {10101, typeof(Authentication) },
                {10108, typeof(Keep_Alive) },
                
            };
        }

        internal static Message Parse(Device client, int messageId)
        {
            if (Messages.ContainsKey(messageId))
                return (Message)Activator.CreateInstance(Messages[messageId], client);

            return null;
        }
    }
}