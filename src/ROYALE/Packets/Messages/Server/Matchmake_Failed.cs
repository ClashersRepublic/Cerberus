using CRepublic.Royale.Extensions.List;
using CRepublic.Royale.Logic;

namespace CRepublic.Royale.Packets.Messages.Server
{
    internal class Matchmake_Failed : Message
    {

        /// <summary>
        /// Initialize a new instance of the <see cref="Matchmake_Failed"/> class.
        /// </summary>
        /// <param name="_Device">The device.</param>
        public Matchmake_Failed(Device _Device) : base(_Device)
        {
            this.Identifier = 24108;
        }

        /// <summary>
        /// Encode this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Data.AddInt(0);
        }
    }
}
