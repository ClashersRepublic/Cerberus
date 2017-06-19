using System;
using CRepublic.Royale.Extensions;
using CRepublic.Royale.Logic;

namespace CRepublic.Royale.Packets.Messages.Server.Authentication
{
    using CRepublic.Royale.Extensions.List;
    using CRepublic.Royale.Logic.Enums;
    using CRepublic.Royale.Packets.Cryptography;

    internal class Pre_Authentification_OK : Message
    {

        internal Pre_Authentification_OK(Device Device) : base(Device)
        {
            this.Identifier = 20100;
            this.Device.PlayerState = Client_State.SESSION_OK;
        }

        /// <summary>
        /// Encodes this message.
        /// </summary>
        internal override void Encode()
        {
            this.Data.AddByteArray(Key.NonceKey);
        }
    }
}