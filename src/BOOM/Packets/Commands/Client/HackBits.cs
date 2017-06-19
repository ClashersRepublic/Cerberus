namespace CRepublic.Boom.Packets.Commands.Client
{
    using System;
    using CRepublic.Boom.Extensions.Binary;
    using CRepublic.Boom.Logic;

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

