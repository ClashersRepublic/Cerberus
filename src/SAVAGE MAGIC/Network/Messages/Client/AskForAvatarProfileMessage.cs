using System;
using System.IO;
using Savage.Magic.Core;

using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.Messages.Client
{
    internal class AskForAvatarProfileMessage : Message
    {
        private long m_vAvatarId;
        private long m_vCurrentHomeId;

        public AskForAvatarProfileMessage(ClashOfClans.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
        }

        public override void Decode()
        {
            m_vAvatarId = Reader.ReadInt64();
            if (Reader.ReadBoolean())
                m_vCurrentHomeId = Reader.ReadInt64();
        }

        public override void Process(Level level)
        {
            var player = ResourcesManager.GetPlayer(m_vAvatarId, false);
            if (player == null)
                return;

            if (m_vAvatarId == player.Avatar.Id)
            {
                player.Tick();
                new AvatarProfileMessage(Client)
                {
                    Level = player
                }.Send();
            }
            else
            {
                Logger.Error("ResourcesManager.GetPlayer returned a player with the wrong ID.");
            }
        }
    }
}
