using System;
using BL.Servers.CR.Extensions;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Messages.Server.Authentication
{
    using BL.Servers.CR.Extensions.List;
    using BL.Servers.CR.Logic.Enums;
    using BL.Servers.CR.Packets.Cryptography;

    internal class Pre_Authentification_OK : Message
    {

        internal Pre_Authentification_OK(Device Device) : base(Device)
        {
            this.Identifier = 20100;
            this.Device.PlayerState = State.SESSION_OK;
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