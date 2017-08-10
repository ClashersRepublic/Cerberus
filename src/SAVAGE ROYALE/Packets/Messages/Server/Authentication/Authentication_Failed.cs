using CRepublic.Royale.Extensions.List;
using CRepublic.Royale.Files;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Logic.Enums;

namespace CRepublic.Royale.Packets.Messages.Server.Authentication
{
    internal class Authentication_Failed : Message
    {
        public Authentication_Failed(Device Device, Reason Reason = Reason.Default) : base(Device)
        {
            this.Identifier = 20103;
            this.Reason = Reason;
            this.Version = 9;
        }


        internal Reason Reason = Reason.Default;
        internal string PatchingHost => "http://192.168.0.5/";

        internal string Message;
        internal string RedirectDomain;
            
        internal override void Encode()
        {
            this.Data.AddInt((int)this.Reason);
            this.Data.AddString(this.Reason == Reason.Patch ? Fingerprint.Json : null);
            this.Data.AddString(this.RedirectDomain);
            this.Data.AddString(this.PatchingHost);
            this.Data.AddString(null);
            this.Data.AddString(this.Message);
            this.Data.AddInt(0);
            this.Data.AddByte(0); this.Data.AddInt(-1);

            this.Data.AddInt(-1);
            this.Data.AddInt(2);
            this.Data.AddInt(0);
            this.Data.AddInt(-1);
        }
    }
}
