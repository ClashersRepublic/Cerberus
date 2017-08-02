using System;
using System.Collections.Generic;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Packets.Messages.Client;

namespace CRepublic.Magic.Packets
{
    internal static class MessageFactory
    {
        public static Dictionary<int, Type> Messages;

         static MessageFactory()
        {
            Messages = new Dictionary<int, Type>
            {
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