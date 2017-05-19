using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.List;

namespace BL.Servers.CoC.Packets.Messages.Server.Battle
{
    internal class Replay_Data : Message
    {
        internal long Battle_ID = 0;
        
        public Replay_Data(Device _Device) : base(_Device)
        {
            this.Identifier = 24114;
        }
        internal override void Encode()
        {
            string Replay = Core.Resources.Battles.Get(this.Battle_ID, Constants.Database, false).Json;
            System.Console.WriteLine(Replay);
            this.Data.AddCompressed(Replay, false);
        }
    }
}
