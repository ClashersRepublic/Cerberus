using System;
using System.IO;
using System.Text;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    // Packet 14201
    internal class FacebookLinkMessage : Message
    {
        public FacebookLinkMessage(ClashOfClans.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(Data)))
            {
                /*
                Console.WriteLine("Boolean -> " + br.ReadBoolean());
                Console.WriteLine("Unknown 1 -> " + br.ReadInt32());
                Console.WriteLine("Unknown 2 -> " + br.ReadInt32());
                Console.WriteLine("Unknown 3 -> " + br.ReadInt32());
                Console.WriteLine("Unknown 4 -> " + br.ReadInt16());
                Console.WriteLine("Token -> " + br.ReadString());
                */
                //Console.WriteLine(Encoding.ASCII.GetString(br.ReadAllBytes()));
            }
        }

        public override void Process(Level level)
        {
        }
    }
}