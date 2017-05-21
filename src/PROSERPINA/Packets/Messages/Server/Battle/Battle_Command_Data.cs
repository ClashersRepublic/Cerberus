using BL.Servers.CR.Core.Network;
using BL.Servers.CR.Extensions.List;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Messages.Server.Battle
{
    internal class Battle_Command_Data : Message
    {
        internal long Sender;

        internal Logic.Slots.Items.Battle Battle = null;
        internal Battle_Command_Data(Device Device) : base(Device)
        {
            this.Identifier = 21902;
        }

        internal override void Encode()
        {
            this.Data.AddVInt(this.Battle.Tick);
            this.Data.AddVInt(this.Battle.Checksum); // D4-A5-CA-94-0C
            this.Data.AddBool(this.Battle.Commands.Count > 0);

            if (this.Battle.Commands.Count > 0)
            {
                this.Data.AddRange(this.Battle.Commands.Dequeue().Handle().Data);
            }
        }
    }
}
