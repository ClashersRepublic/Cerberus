using BL.Servers.CR.Logic.Slots.Items;
using System;
using System.Linq;
using System.Text;

namespace BL.Servers.CR.Extensions
{
    internal static class GameUtils
    {
        internal const int SEARCH_TAG_LENGTH = 14;

        internal static readonly char[] SEARCH_TAG_CHARS = "0289PYLQGRJCUV".ToCharArray();

        internal static string GetHashtag(long Identifier)
        {
            if (GameUtils.GetHighID(Identifier) <= 255)
            {
                StringBuilder Stringer = new StringBuilder();
                int Count = 11;
                Identifier = ((long)GameUtils.GetLowID(Identifier) << 8) + GameUtils.GetHighID(Identifier);

                while (++Count > 0)
                {
                    Stringer.Append(GameUtils.SEARCH_TAG_CHARS[(int)(Identifier % GameUtils.SEARCH_TAG_LENGTH)]);
                    Identifier /= GameUtils.SEARCH_TAG_LENGTH;
                    if (Identifier <= 0)
                    {
                        break;
                    }
                }

                return new string(Stringer.Append("#").ToString().Reverse().ToArray());
            }

            return string.Empty;
        }

        internal static int GetLowID(long Identifier)
        {
            return (int)(Identifier & 0xFFFFFFFF);
        }

        internal static int GetHighID(long Identifier)
        {
            return (int)(Identifier >> 32);
        }

        internal static double WinTrophies(this Battle _Battle)
        {
            double Difference = (_Battle.Player1.Trophies - _Battle.Player2.Trophies) < 0
                ? +(_Battle.Player1.Trophies - _Battle.Player2.Trophies)
                : (_Battle.Player1.Trophies - _Battle.Player2.Trophies);
            //double Difference = (AttackerTrophies - DefenderTrophies) < 0 ? +(DefenderTrophies - AttackerTrophies) : (AttackerTrophies - DefenderTrophies);
            if (Difference >= 13 && Difference <= 34)
            {
                return Math.Round(-0.0794 * (_Battle.Player1.Trophies - _Battle.Player2.Trophies) + 29.35838);
            }
            return Core.Resources.Random.Next(10, 15);

        }

        internal static double LoseTrophies(this Battle _Battle)
        {
            if (_Battle.Player2.Trophies >= 1000 && _Battle.Player2.Trophies >= 1000)
            {
                return Math.Round(0.0531 * (_Battle.Player1.Trophies - _Battle.Player2.Trophies) + 19.60453);
            }
            return Core.Resources.Random.Next(10, 15);

        }    
    }
}