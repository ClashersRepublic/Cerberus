using System;
using System.Collections.Generic;
using System.Net;
using BL.Networking.Lyra.Core;
using BL.Networking.Lyra.Enums;
using BL.Networking.Lyra.Extensions;

namespace BL.Networking.Lyra.Crypto
{
    internal class Keys
    {
        internal static byte[] Private_Key => "840D952E959E4CAFBAED7BEA72559714679F6612D1DD347D41D5F7BD9A1510A6".ToByteArray();
        internal static byte[] Modded_Key => "E10C80A801076E9278299FCDEDAE7CA6CF4075D11301C295D49D272E35420D28".ToByteArray();

        internal static byte[] Original_Key = new byte[Constants.Key_Length];

        internal static void SetKey()
        {
            switch (Constants.Game)
            {
                case Game.CLASH_OF_CLANS:
                    Original_Key = "".ToByteArray();
                    break;
                case Game.BOOM_BEACH:
                    Original_Key = "".ToByteArray();
                    break;
                case Game.CLASH_ROYALE:
                    Original_Key = "9E6657F2B419C237F6AEEF37088690A642010586A7BD9018A15652BAB8370F4F".ToByteArray();
                    break;
                case Game.HAY_DAY:
                    Original_Key = "".ToByteArray();
                    break;
                default:
                    break;
            }

            Console.WriteLine($"Public key set to '{Original_Key.ToHexString()}'" + Environment.NewLine);
        }
    }
}
