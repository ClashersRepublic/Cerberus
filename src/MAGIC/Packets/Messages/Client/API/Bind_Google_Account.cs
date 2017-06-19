using System;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;

namespace CRepublic.Magic.Packets.Messages.Client.API
{
    internal class Bind_Google_Account : Message
    {
        internal byte Unknown;

        internal string GoogleID;
        internal string Token;

        public Bind_Google_Account(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Bind_Google_Account.
        }

        internal override void Decode()
        {
            this.Unknown = this.Reader.ReadByte();
            this.GoogleID = this.Reader.ReadString(); // 103419370274411296720
            this.Token = this.Reader.ReadString(); // -4/IN0G9ON8fll-HG3fZBj-nkbYuaDHIxSnr-n7_1g9EVQ
        }

        internal override void Process()
        {
            if (!string.IsNullOrEmpty(this.GoogleID) && !string.IsNullOrEmpty(this.Token))
            {
                this.Device.Player.Avatar.Google.Identifier = this.GoogleID;
                this.Device.Player.Avatar.Google.Token = this.Token;

                this.Device.Player.Avatar.Google.Connect();
            }
        }
    }
}