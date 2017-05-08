using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Packets.Messages.Server.Authentication;

namespace BL.Servers.CoC.Packets.Messages.Client.Authentication
{
    internal class Pre_Authentication : Message
    {
        internal int AppStore;
        internal int DeviceSO;
        internal int KeyVersion;
        internal int Protocol;
        internal int Major;
        internal int Minor;
        internal int Revision;

        internal string Hash;

        public Pre_Authentication(Device Device, Reader Reader) : base(Device, Reader)
        {
           this.Device.State = State.SESSION;
        }

        internal override void Decode()
        {
            this.Protocol = this.Reader.ReadInt32();
            this.KeyVersion = this.Reader.ReadInt32();
            this.Major = this.Reader.ReadInt32();
            this.Revision = this.Reader.ReadInt32();
            this.Minor = this.Reader.ReadInt32();
            this.Hash = this.Reader.ReadString();
            this.DeviceSO = this.Reader.ReadInt32();
            this.AppStore = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            //if (this.Major == (int)CVersion.Major && this.Minor == (int)CVersion.Minor)
            {
                //if (!Constants.Maintenance)
                {
                    // if (string.Equals(this.Hash, Fingerprint.Sha))
                    // {
                    new Pre_Authentication_OK(this.Device).Send();
                    //}
                    //else
                    //  new Authentification_Failed(this.Device, Reason.Patch).Send();
                    //   }
                    // else
                    //   new Authentification_Failed(this.Device, Reason.Maintenance).Send();
                }
                // else
                //   new Authentification_Failed(this.Device, Reason.Update).Send();
            }
        }
    }
}