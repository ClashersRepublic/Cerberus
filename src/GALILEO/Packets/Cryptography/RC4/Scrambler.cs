using System;

namespace BL.Servers.CoC.Packets.Cryptography.RC4
{
    internal class Scrambler
    {
        internal Scrambler(ulong seed)
        {
            IX = 0;
            Buffer = SeedBuffer(seed);
        }

        internal int IX { get; set; }
        internal ulong[] Buffer { get; set; }

        internal static ulong[] SeedBuffer(ulong seed)
        {
            var buffer = new ulong[624];
            for (var i = 0; i < 624; i++)
            {
                buffer[i] = seed;
                seed = (1812433253 * ((seed ^ RShift(seed, 30)) + 1)) & 0xFFFFFFFF;
            }
            return buffer;
        }

        internal int GetByte()
        {
            var x = (ulong)GetInt();
            if (IsNeg(x)) x = Negate(x);
            return (int)(x % 256);
        }

        internal int GetInt()
        {
            if (IX == 0) MixBuffer();
            var val = Buffer[IX];

            IX = (IX + 1) % 624;
            val ^= RShift(val, 11) ^ LShift((val ^ RShift(val, 11)), 7) & 0x9D2C5680;
            return (int)(RShift((val ^ LShift(val, 15L) & 0xEFC60000), 18L) ^ val ^ LShift(val, 15L) & 0xEFC60000);
        }

        internal void MixBuffer()
        {
            var i = 0;
            var j = 0;
            while (i < 624)
            {
                i += 1;
                var v4 = (Buffer[i % 624] & 0x7FFFFFFF) + (Buffer[j] & 0x80000000);
                var v6 = RShift(v4, 1) ^ Buffer[(i + 396) % 624];
                if ((v4 & 1) != 0) v6 ^= 0x9908B0DF;
                Buffer[j] = v6;
                j += 1;
            }
        }

        internal static ulong RShift(ulong num, ulong n)
        {
            var highbits = (ulong)0;
            if ((num & Pow(2, 31)) != 0) highbits = (Pow(2, n) - 1) * Pow(2, 32 - n);
            return (num / Pow(2, n)) | highbits;
        }

        internal static ulong LShift(ulong num, ulong n)
        {
            return (num * Pow(2, n)) % Pow(2, 32);
        }

        internal static bool IsNeg(ulong num)
        {
            return (num & (ulong)Math.Pow(2, 31)) != 0;
        }

        internal static ulong Negate(ulong num)
        {
            return (~num) + 1;
        }

        internal static ulong Pow(ulong x, ulong y)
        {
            return (ulong)Math.Pow(x, y);
        }
    }
}
