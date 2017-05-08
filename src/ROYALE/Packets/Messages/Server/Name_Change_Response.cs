using BL.Servers.CR.Extensions.List;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Messages.Server
{
    internal class Name_Change_Response : Message
    {
        internal string Name;

        public Name_Change_Response(Device Device, string Name) : base(Device)
        {
            this.Identifier = 20300;
            this.Name = Name;

        }

        // Error Codes:
        // 1 - Invalid Name
        // 2 - Name Too Short
        // 3 - Changed More than Once

        internal override void Encode()
        {
            this.Data.AddBool(false);
            this.Data.AddInt(1);
            this.Data.AddString(null);
        }
    }
}
