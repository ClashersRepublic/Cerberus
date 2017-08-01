using System;
using System.Collections.Generic;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.List;
using CRepublic.Magic.Logic.Structure.Slots.Items;
using CRepublic.Magic.Packets.Messages.Server;
using Newtonsoft.Json;

namespace CRepublic.Magic.Logic.Structure.Slots
{
    internal class Inbox 
    {
        internal Player Player;
        [JsonProperty("seed")] internal int Seed;
        [JsonProperty("slots")] internal List<Mail> Slots;

        internal object Gate = new object();

        internal Inbox()
        {
            this.Slots = new List<Mail>(100);
        }

        internal Inbox(Player Player, int Limit = 100)
        {
            this.Player = Player;
            this.Slots = new List<Mail>(Limit);
        }

        internal void Add(Mail Message)
        {
            lock (this.Gate)
            {
                Message.Message_LowID = Seed++;
                Message.New = 2;

                if (this.Slots.Count < this.Slots.Capacity)
                {
                    this.Slots.Add(Message);
                }
                else
                {
                    this.Slots.RemoveAt(0);
                    this.Slots.Add(Message);
                }
            }

            var Avatar = Core.Players.Get(Player.UserId, false);
            if (Avatar?.Device != null)
            {
                new Avatar_Stream_Entry(Avatar.Device, Message).Send();
            }
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> Packet = new List<byte>();

                Packet.AddInt(this.Slots.Count);

                foreach (Mail Message in this.Slots)
                {
                    Packet.AddRange(Message.ToBytes);
                }

                return Packet.ToArray();
            }
        }

        internal void Update()
        {
            foreach (Mail Entry in this.Slots)
            {
                /* if (Entry.Outdated)
                {
                    this.Remove(Entry);
                } */
            }
        }

    }
}
