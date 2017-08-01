using Savage.Magic;
using Savage.Magic.Core;
using Savage.Magic;
using Savage.Magic.Network.Messages.Client;
using Magic.Packets.Messages.Client;
using System;
using System.Collections.Generic;
using System.IO;

namespace Savage.Magic.Network
{
    internal static class MessageFactory
    {
        private static readonly Dictionary<int, Type> _messages;

        static MessageFactory()
        {
            _messages = new Dictionary<int, Type>
            {
                
            };
        }

        public static Message Read(Client client, PacketReader reader, int messageId)
        {
            if (_messages.ContainsKey(messageId))
                return (Message)Activator.CreateInstance(_messages[messageId], client, reader);

            return null;
        }
    }
}
