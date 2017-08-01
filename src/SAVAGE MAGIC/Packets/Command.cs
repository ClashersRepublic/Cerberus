using Savage.Magic;
using Savage.Magic.Logic;

namespace Savage.Magic.Network
{
    internal class Command
    {
        public const int MaxEmbeddedDepth = 10;

        internal int Depth { get; set; }

        public virtual byte[] Encode() => null;

        public virtual void Execute(Level level)
        {
            // Space
        }
    }
}
