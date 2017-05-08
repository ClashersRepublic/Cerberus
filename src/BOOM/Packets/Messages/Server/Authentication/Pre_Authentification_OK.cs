using BL.Servers.BB.Logic;

namespace BL.Servers.BB.Packets.Messages.Server.Authentication
{
    using BL.Servers.BB.Extensions.List;
    using BL.Servers.BB.Logic.Enums;

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
            this.Data.AddInt(24);
            this.Data.AddRange(Key.NonceKey);
        }
    }
}