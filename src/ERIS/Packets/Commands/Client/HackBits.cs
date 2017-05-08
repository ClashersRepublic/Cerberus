namespace BL.Servers.BB.Packets.Commands.Client
{
    using System;
    using BL.Servers.BB.Extensions.Binary;
    using BL.Servers.BB.Logic;

    internal class HackBits : Command
    {

        public HackBits(Reader _Reader, Device _Client, int _ID) : base(_Reader, _Client, _ID)
        {

        }

        internal override void Decode()
        {
            Console.WriteLine(BitConverter.ToString(this.Reader.ReadFully()));
        }
    }
}

