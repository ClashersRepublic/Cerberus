using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Core;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.External.Blake2B;
using BL.Servers.CoC.External.Sodium;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Packets.Messages.Server.Authentication;
using BL.Servers.CoC.Packets.Messages.Server;

namespace BL.Servers.CoC.Packets.Messages.Client.Authentication
{
    internal class Authentification : Message
    {
        public Authentification(Device device, Reader reader) : base(device, reader)
        {
            this.Device.State = State.LOGIN;
        }

        internal StringBuilder Reason;
        internal long UserId;

        internal string Token, MasterHash, Language, UDID;      

        internal int Major, Minor, Revision;

        internal string[] ClientVersion;

        internal override void Decrypt()
        {
            byte[] Buffer = this.Reader.ReadBytes(this.Length);
            this.Device.Keys.PublicKey = Buffer.Take(32).ToArray();

            Blake2BHasher Blake = new Blake2BHasher();

            Blake.Update(this.Device.Keys.PublicKey);
            Blake.Update(Key.PublicKey);

            this.Device.Keys.RNonce = Blake.Finish();

            Buffer = Sodium.Decrypt(Buffer.Skip(32).ToArray(), this.Device.Keys.RNonce, Key.PrivateKey, this.Device.Keys.PublicKey);
            this.Device.Keys.SNonce = Buffer.Skip(24).Take(24).ToArray();
            this.Reader = new Reader(Buffer.Skip(48).ToArray());

            this.Length = (ushort)Buffer.Length;

        }

        internal override void Decode()
        {
            this.UserId = this.Reader.ReadInt64();

            this.Token = this.Reader.ReadString();

            this.Major = this.Reader.ReadInt32();
            this.Minor = this.Reader.ReadInt32();
            this.Revision = this.Reader.ReadInt32();

            this.MasterHash = this.Reader.ReadString();

            this.Reader.ReadString();

            this.Device.AndroidID = this.Reader.ReadString();
            this.Device.MACAddress = this.Reader.ReadString();
            this.Device.Model = this.Reader.ReadString();

            this.Reader.Seek(4);    // 2000001

            this.Language = this.Reader.ReadString();
            this.Device.OpenUDID = this.Reader.ReadString();
            this.Device.OSVersion = this.Reader.ReadString();

            this.Device.Android = this.Reader.ReadBoolean();

            this.Reader.ReadString();

            this.Device.AndroidID = this.Reader.ReadString();

            this.Reader.ReadString();

            this.Device.Advertising = this.Reader.ReadBoolean();

            this.Reader.ReadString();

            this.Device.ClientSeed = this.Reader.ReadUInt32();

            this.Reader.ReadByte();
            this.Reader.ReadString();
            this.Reader.ReadString();

            this.ClientVersion = this.Reader.ReadString().Split(',');

        }

        internal override void Process()
        {
            if (this.UserId == 0)
            {
                this.Device.Player = Resources.Players.New();

                if (this.Device.Player != null)
                {
                    if (this.Device.Player.Avatar.Locked)
                    {
                        new Authentication_Failed(this.Device, Logic.Enums.Reason.Locked).Send();
                    }
                    else
                    {
                        this.Login();
                    }
                }
                else
                {
                   new Authentication_Failed(this.Device, Logic.Enums.Reason.Pause).Send();
                }
            }
            else if (this.UserId > 0)
            {
                this.Device.Player = Resources.Players.Get(this.UserId, Constants.Database);
                if (this.Device.Player != null)
                {
                    if (string.Equals(this.Token, this.Device.Player.Avatar.Token))
                    {
                        if (this.Device.Player.Avatar.Locked)
                        {
                            new Authentication_Failed(this.Device, Logic.Enums.Reason.Locked).Send();
                        }
                        else if (this.Device.Player.Avatar.Banned)
                        {
                            this.Reason = new StringBuilder();
                            this.Reason.AppendLine(
                                "Your Player have been banned on our servers, please contact one of the server staff with these following informations if you are not satisfied with the ban:");
                            this.Reason.AppendLine();
                            this.Reason.AppendLine("Your Player Name         : " + this.Device.Player.Avatar.Name);
                            this.Reason.AppendLine("Your Player ID           : " + this.Device.Player.Avatar.UserId);
                            this.Reason.AppendLine("Your Player Tag          : " + GameUtils.GetHashtag(this.Device.Player.Avatar.UserId));
                            this.Reason.AppendLine("Your Player Ban Duration : " + Math.Round((this.Device.Player.Avatar.BanTime.AddDays(3) - DateTime.UtcNow).TotalDays,  3) + " Day");
                            this.Reason.AppendLine("Your Player Unlock Date  : " + this.Device.Player.Avatar.BanTime.AddDays(3));
                            this.Reason.AppendLine();

                            new Authentication_Failed(this.Device, Logic.Enums.Reason.Banned)
                            {
                                Message = Reason.ToString()
                            }.Send();
                        }
                        else
                        {
                            this.Login();
                        }
                    }
                    else
                    {
                        new Authentication_Failed(this.Device, Logic.Enums.Reason.Locked).Send();
                    }
                }
                else
                {
                    this.Reason = new StringBuilder();
                    this.Reason.AppendLine("Your Device have been block from accessing our servers due to invalid Id, please  clear your game data or contact one of the BarbarianLand staff with these following informations if you are not able to clear you game data :");
                    this.Reason.AppendLine("Your Device IP         : " + this.Device.IPAddress + ".");
                    this.Reason.AppendLine("Your Requested ID       : " + this.UserId + ".");
                    this.Reason.AppendLine();

                    new Authentication_Failed(this.Device, Logic.Enums.Reason.Banned)
                    {
                        Message = Reason.ToString()
                    }.Send();
                }
            }
        }
        internal void Login()
        {
            this.Device.Player.Client = this.Device;
            this.Device.Player.Avatar.Region = Resources.Region.GetIpCountry(this.Device.Player.Avatar.IpAddress = this.Device.IPAddress);

            Resources.GChat.Add(this.Device);
            Resources.PRegion.Add(this.Device.Player);

             new Authentication_OK(this.Device).Send();
             new Own_Home_Data(this.Device).Send();
            new Avatar_Stream(this.Device).Send();
            //new Shutdown_Started(this.Device, 30).Send();
            //new Inbox_Data(this.Device).Send();

        }
    }
}
