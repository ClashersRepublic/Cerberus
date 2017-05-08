namespace BL.Servers.CR.Packets.Messages.Server.Stream
{
    using BL.Servers.CR.Extensions.List;
    using BL.Servers.CR.Logic;

    internal class Avatar_Stream_Entry : Message
    {
        internal Avatar_Stream_Entry(Device Device) : base(Device)
        {
            this.Identifier = 24412;
        }

        internal override void Encode()
        {
            this.Data.AddInt(6);
            this.Data.Add(1);
            this.Data.AddLong(0);
            this.Data.AddString("BL Server test");
            this.Data.AddInt(40);
            this.Data.AddInt(12);
            this.Data.AddInt(10);
            this.Data.Add(1);
            this.Data.AddString("Test lol");
            this.Data.Add(1);
            this.Data.AddLong(1);
            this.Data.AddLong(0);
            this.Data.AddString("BarbarianLand");
            this.Data.AddInt(0);

        }
    }
}
