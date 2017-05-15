using BL.Servers.CoC.Core;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Files;
using BL.Servers.CoC.Files.CSV_Logic;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Logic.Structure.Slots.Items;
using BL.Servers.CoC.Packets.Commands.Server;
using BL.Servers.CoC.Packets.Messages.Server;
using BL.Servers.CoC.Packets.Messages.Server.Clans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CoC.Packets.Messages.Client.Clans
{
    internal class Donate_Alliance_Unit : Message
    {
        internal Characters Troop;
        internal Spells Spell;
        internal int GlobalId;
        internal int StreamHighId;
        internal int StreamLowId;
        internal bool PaidTroop;

        internal bool IsSpell;
        public Donate_Alliance_Unit(Device Device, Reader Reader) : base(Device, Reader)
        {

        }
        internal override void Decode()
        {
            this.Reader.ReadInt32();
            this.GlobalId = this.Reader.ReadInt32();
            if (GlobalId < 4000000)
            {
                this.IsSpell = true;
                this.Spell = CSV.Tables.Get(Gamefile.Buildings).GetDataWithID(this.Reader.ReadInt32()) as Spells;
            }
            else
            {
                this.Troop = CSV.Tables.Get(Gamefile.Buildings).GetDataWithID(this.Reader.ReadInt32()) as Characters;
            }

            this.StreamHighId = this.Reader.ReadInt32();
            this.StreamLowId = this.Reader.ReadInt32();
            this.PaidTroop = this.Reader.ReadByte() > 0;
        }

        internal override void Process()
        {
            ShowValues();
            Clan Alliance = Resources.Clans.Get(this.Device.Player.Avatar.ClanId, Constants.Database, false);
            Entry Stream = Alliance.Chats.Get(this.StreamLowId);
            if (Stream != null)
            {
                Level Receiver = Resources.Players.Get(Stream.Sender_ID, Constants.Database, false);

                if (IsSpell)
                {
                    if (Stream.Max_Spells >= Stream.Max_Spells + this.Spell.HousingSpace[0])
                    {
                        var Spell_Level = this.Device.Player.Avatar.GetUnitUpgradeLevel(this.Spell);
                        Stream.AddSpell(this.Device.Player.Avatar.UserId, this.GlobalId, 1, Spell_Level);
                        Stream.Used_Space_Spells += this.Spell.HousingSpace[0];
                        Receiver.Avatar.Castle_Used_SP += this.Spell.HousingSpace[0];
                        Receiver.Avatar.AddCastleSpell(this.GlobalId, 1, Spell_Level);

                        new Server_Commands(this.Device) { Command = new Donate_Troop_Callback(this.Device) { SteamHighId = this.StreamHighId, StreamLowId = this.StreamLowId, TroopGlobalId = this.Spell.GetGlobalID() }.Handle() }.Send();

                        if (Receiver.Client != null)
                        {
                            new Server_Commands(Receiver.Client) { Command = new Receive_Troop_Callback(Receiver.Client) { TroopGlobalId = this.Spell.GetGlobalID(), TroopLevel = Spell_Level, DonatorName = this.Device.Player.Avatar.Name }.Handle() }.Send();
                        }

                        foreach (Member Member in Alliance.Members.Values)
                        {
                            if (Member.Connected)
                            {
                                new Alliance_Stream_Entry(Member.Player.Client, Stream).Send();
                            }
                        }
                    }
                    else
                    {
                        if (Stream.Max_Troops >= Stream.Max_Troops + this.Troop.HousingSpace)
                        {

                            var Unit_Level = this.Device.Player.Avatar.GetUnitUpgradeLevel(this.Troop);
                            Stream.AddTroop(this.Device.Player.Avatar.UserId, this.GlobalId, 1, Unit_Level);
                            Stream.Used_Space_Troops += this.Troop.HousingSpace;

                            Receiver.Avatar.Castle_Used += this.Troop.HousingSpace;
                            Receiver.Avatar.AddCastleTroop(this.GlobalId, 1, Unit_Level);

                            new Server_Commands(this.Device) { Command = new Donate_Troop_Callback(this.Device) { SteamHighId = this.StreamHighId, StreamLowId = this.StreamLowId, TroopGlobalId = this.Troop.GetGlobalID() }.Handle() }.Send();

                            if (Receiver.Client != null)
                            {
                                new Server_Commands(Receiver.Client) { Command = new Receive_Troop_Callback(Receiver.Client) { TroopGlobalId = this.Troop.GetGlobalID(), TroopLevel = Unit_Level, DonatorName = this.Device.Player.Avatar.Name }.Handle() }.Send();
                            }

                            foreach (Member Member in Alliance.Members.Values)
                            {
                                if (Member.Connected)
                                {
                                    new Alliance_Stream_Entry(Member.Player.Client, Stream).Send();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
