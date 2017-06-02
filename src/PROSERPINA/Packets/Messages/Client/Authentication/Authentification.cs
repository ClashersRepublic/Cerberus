using System;
using BL.Servers.CR.Core;
using BL.Servers.CR.Packets.Messages.Server;
using BL.Servers.CR.Packets.Messages.Server.Stream;

namespace BL.Servers.CR.Packets.Messages.Client.Authentication
{
    using System.Linq;
    using System.Text;
    using BL.Servers.CR.Core.Network;
    using BL.Servers.CR.Library.Blake2B;
    using BL.Servers.CR.Library.Sodium;
    using BL.Servers.CR.Logic;
    using BL.Servers.CR.Logic.Enums;
    using BL.Servers.CR.Packets.Messages.Server.Authentication;
    using BL.Servers.CR.Extensions.Binary;
    using BL.Servers.CR.Files;

    internal class Authentification : Message
    {
        public Authentification(Device device, Reader reader) : base(device, reader)
        {
            this.Device.PlayerState = State.LOGIN;
        }


        internal StringBuilder Reason;
        internal long UserId;

        internal string Token;
        internal string MasterHash;
        internal string Language;
        internal string UDID;

        internal override void DecryptSodium()
        {
            byte[] Buffer = this.Reader.ReadBytes(this.Length);
            this.Device.Crypto.PublicKey = Buffer.Take(32).ToArray();

            Blake2BHasher Blake = new Blake2BHasher();

            Blake.Update(this.Device.Crypto.PublicKey);
            Blake.Update(Packets.Cryptography.Key.PublicKey);

            this.Device.Crypto.RNonce = Blake.Finish();

            Buffer = Sodium.Decrypt(Buffer.Skip(32).ToArray(), this.Device.Crypto.RNonce, Packets.Cryptography.Key.PrivateKey, this.Device.Crypto.PublicKey);
            this.Device.Crypto.SNonce = Buffer.Skip(24).Take(24).ToArray();
            this.Reader = new Reader(Buffer.Skip(48).ToArray());

            this.Length = (ushort) Buffer.Length;

        }

        internal override void Decode()
        {
            this.UserId = this.Reader.ReadInt64();

            this.Token = this.Reader.ReadString();

            this.Device.Major = this.Reader.ReadVInt();
            this.Device.Minor = this.Reader.ReadVInt();
            this.Device.Revision = this.Reader.ReadVInt();

            this.MasterHash = this.Reader.ReadString();

            this.UDID = this.Reader.ReadString();

            this.Device.OpenUDID = this.Reader.ReadString();
            this.Device.MACAddress = this.Reader.ReadString();
            this.Device.Model = this.Reader.ReadString();
            this.Device.AdvertiseID = this.Reader.ReadString();

            this.Device.OSVersion = this.Reader.ReadString();

            this.Reader.ReadByte();

            this.Reader.Seek(4);

            this.Device.AndroidID = this.Reader.ReadString();
            this.Language = this.Reader.ReadString();

            this.Reader.ReadByte();
            this.Reader.ReadByte();

            this.Reader.ReadString();

            this.Reader.ReadByte();

            this.Reader.Seek(4);

            this.Reader.ReadByte();

            this.Reader.Seek(17);
        }

        internal override void Process()
        {
            if (string.Equals(this.MasterHash, Fingerprint.Sha))
            {
                if (this.UserId == 0)
                {
                    this.Device.Player = Resources.Players.New();

                    if (this.Device.Player != null)
                    {
                        this.Login();
                    }
                    else
                    {
                        new Authentification_Failed(this.Device, Logic.Enums.Reason.Pause).Send();
                    }
                }
                else if (this.UserId > 0)
                {
                    this.Device.Player = Resources.Players.Get(this.UserId);

                    if (this.Device.Player != null)
                    {
                        if (string.Equals(this.Token, this.Device.Player.Token))
                        {
                            if (this.Device.Player.Locked)
                            {
                                new Authentification_Failed(this.Device, Logic.Enums.Reason.Locked).Send();
                            }
                            else if (this.Device.Player.Banned)
                            {
                                this.Reason = new StringBuilder();
                                this.Reason.AppendLine("You have been banned from our servers, please contact one of the BarbarianLand staff members with these following information if you are not happy with the ban:");
                                this.Reason.AppendLine();
                                this.Reason.AppendLine("Your Name: " + this.Device.Player.Username + ".");
                                this.Reason.AppendLine("Your ID: " + this.Device.Player.UserHighId + "-" + this.Device.Player.UserLowId + ".");
                                this.Reason.AppendLine("Your Ban Duration: " + Math.Round((this.Device.Player.BanTime - DateTime.UtcNow).TotalDays, 3) + " Day.");
                                this.Reason.AppendLine("Your Unlock Date: " + this.Device.Player.BanTime);
                                this.Reason.AppendLine();

                                new Authentification_Failed(this.Device, Logic.Enums.Reason.Banned)
                                {
                                    Message = this.Reason.ToString()
                                }.Send();
                            }
                            else
                            {
                                this.Login();
                            }
                        }
                        else
                        {
                            new Authentification_Failed(this.Device, Logic.Enums.Reason.Locked).Send();
                        }
                    }
                    else
                    {
                        this.Reason = new StringBuilder();
                        this.Reason.AppendLine("You have been block from accessing our servers due to invalid id, please clear your game data or contact one of the BarbarianLand staff members with these following information if you are not able to clear you game data:");
                        this.Reason.AppendLine("Your IP: " + this.Device.IPAddress + ".");
                        this.Reason.AppendLine("Your ID: " + this.UserId + ".");
                        this.Reason.AppendLine();

                        new Authentification_Failed(this.Device, Logic.Enums.Reason.Banned)
                        {
                            Message = Reason.ToString()
                        }.Send();
                    }
                }
            }
            else
            {
                new Authentification_Failed(this.Device, Logic.Enums.Reason.Patch).Send();
            }
        }

        internal void Login()
        {
            this.Device.Player.Device = this.Device;

            if (this.Language.Length > 2)
                this.Device.Player.Region = this.Language.Substring(this.Language.Length - 2);

            new Authentification_OK(this.Device).Send();
            new Own_Home_Data(this.Device).Send();
            //new Inbox_Data(this.Device).Send();
        }
    }
}
