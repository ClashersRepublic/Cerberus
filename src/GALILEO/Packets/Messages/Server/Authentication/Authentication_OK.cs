using System.Collections.Generic;
using System.Linq;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.External.Blake2B;
using BL.Servers.CoC.External.Sodium;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using System;
using BL.Servers.CoC.Packets.Cryptography;

namespace BL.Servers.CoC.Packets.Messages.Server.Authentication
{
    internal class Authentication_OK : Message
    {
        public Authentication_OK(Device client) : base(client)
        {
            this.Identifier = 20104;
            this.Device.State = State.LOGGED;
        }

        internal int ServerBuild;
        internal int ServerMajorVersion;
        internal int ContentVersion;

        internal override void Encode()
        {
            Player avatar = this.Device.Player.Avatar;
            this.Data.AddLong(avatar.UserId);
            this.Data.AddLong(avatar.UserId);

            this.Data.AddString(avatar.Token);

            this.Data.AddString(Convert.ToString(avatar.Facebook.Identifier));
            this.Data.AddString(avatar.Gamecenter.Identifier);


            this.Data.AddInt(ServerMajorVersion);
            this.Data.AddInt(ServerBuild);
            this.Data.AddInt(ContentVersion);

            this.Data.AddString("prod");

            this.Data.AddInt(avatar.Login_Count++); //Session Count
            this.Data.AddInt((int)avatar.PlayTime.TotalSeconds); //Playtime Second
            this.Data.AddInt(0);
            
            this.Data.AddString(Logic.Structure.API.Facebook.ApplicationID);

            this.Data.AddString(TimeUtils.ToJavaTimestamp(avatar.LastSave).ToString()); // 14 75 26 87 86 11 24 33
            this.Data.AddString(TimeUtils.ToJavaTimestamp(avatar.Created).ToString()); // 14 78 03 95 03 10 0

            this.Data.AddInt(0);
            this.Data.AddString(avatar.Google.Identifier);
            this.Data.AddString(avatar.Region);
            this.Data.AddString(null);
            this.Data.AddString(null);

        }

        internal override void EncryptPepper()
        {
            Blake2BHasher blake = new Blake2BHasher();

            blake.Update(this.Device.Keys.SNonce);
            blake.Update(this.Device.Keys.PublicKey);
            blake.Update(Key.PublicKey);

            byte[] Nonce = blake.Finish();
            byte[] encrypted = this.Device.Keys.RNonce.Concat(this.Device.Keys.PublicKey).Concat(this.Data).ToArray();

            this.Data = new List<byte>(Sodium.Encrypt(encrypted, Nonce, Key.PrivateKey, this.Device.Keys.PublicKey));

            this.Length = (ushort)this.Data.Count;
        }
    }
}
