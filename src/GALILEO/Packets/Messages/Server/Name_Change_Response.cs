using CRepublic.Magic.Extensions.List;
using CRepublic.Magic.Logic;

namespace CRepublic.Magic.Packets.Messages.Server
{
    internal class Name_Change_Response : Message
    {
        internal string Name;

        public Name_Change_Response(Device Device, string Name) : base(Device)
        {
            this.Identifier = 20300;
            this.Name = Name;

        }

        internal override void Encode()
        {
            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddString(this.Name);
        }
    }
}
