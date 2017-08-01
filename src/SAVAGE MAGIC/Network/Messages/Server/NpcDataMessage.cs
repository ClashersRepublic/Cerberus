using System;
using System.Collections.Generic;
using System.Text;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Client;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class NpcDataMessage : Message
    {
        public NpcDataMessage(ClashOfClans.Client client, Level level, AttackNpcMessage cnam) : base(client)
        {
            MessageType = 24133;
            Player = level;
            LevelId = cnam.LevelId;
            JsonBase = ObjectManager.NpcLevels[LevelId];

        }

        public override void Encode()
        {
            var ownerHome = new Home(Player.Avatar.Id);
            ownerHome.SetShieldTime(Player.Avatar.RemainingShieldTime);
            ownerHome.SetHomeJson(JsonBase);

            Player.Avatar.State = Avatar.UserState.PVE;
            var data = new List<byte>();

            data.AddInt32(0);
            data.AddInt32((int)Player.Time.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            data.AddRange(ownerHome.Encode());
            data.AddRange(Player.Avatar.Encode());
            data.AddInt32(LevelId);

            Encrypt(data.ToArray());
        }

        public string JsonBase { get; set; }
        public int LevelId { get; set; }
        public Level Player { get; set; }
    }
}