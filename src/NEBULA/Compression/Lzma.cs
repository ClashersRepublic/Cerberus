namespace CR.Assets.Editor.Compression
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using SevenZip;

    using Decoder = SevenZip.Compression.LZMA.Decoder;
    using Encoder = SevenZip.Compression.LZMA.Encoder;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;

    internal class Lzma
    {
        private const int Dictionary = 1 << 24;
        private const int PosStateBits = 2;
        private const int LitContextBits = 4;
        private const int LitPosBits = 0;
        private const int Algorithm = 2;
        private const int NumFastBytes = 32;
        private const string Mf = "bt4";
        private const bool Eos = false;

        internal static void CompressCoC(string file, string outputlocation)
        {
            var encoder = new Encoder();
            using (var input = new FileStream(file, FileMode.Open))
            {
                using (var output = new FileStream(outputlocation, FileMode.Create, FileAccess.Write))
                {
                    CoderPropID[] propIDs =
                    {
                        CoderPropID.DictionarySize,
                        CoderPropID.PosStateBits,
                        CoderPropID.LitContextBits,
                        CoderPropID.LitPosBits,
                        CoderPropID.Algorithm,
                        CoderPropID.NumFastBytes,
                        CoderPropID.MatchFinder,    
                        CoderPropID.EndMarker
                    };

                    object[] properties =
                    {
                        Dictionary,
                        PosStateBits,
                        3,
                        //LitContextBits,
                        LitPosBits,
                        Algorithm,
                        NumFastBytes,
                        Mf,
                        Eos
                    };
                    encoder.SetCoderProperties(propIDs, properties);
                    encoder.WriteCoderProperties(output);
                    output.Write(BitConverter.GetBytes(input.Length), 0, 4);

                    encoder.Code(input, output, input.Length, -1, null);
                    output.Flush();
                    output.Dispose();
                }
                input.Dispose();
            }
        }
            
        public static byte[] HexaToBytes(string _Value)
        {
            string _Tmp = _Value.Contains("-") ? _Value.Replace("-", string.Empty) : _Value.Replace(" ", string.Empty);
            return Enumerable.Range(0, _Tmp.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(_Tmp.Substring(x, 2), 16)).ToArray();
        }

        internal static void CompressCR(string file, string outputlocation)
        {
            File.Copy(file, file += ".clone");
            byte[] hash;
            using (var md5 = MD5.Create())
            {
                hash = md5.ComputeHash(File.ReadAllBytes(file));
            }

            var encoder = new Encoder();
            using (var input = new FileStream(file, FileMode.Open))
            {
                using (var output = new FileStream(outputlocation, FileMode.Create, FileAccess.Write))
                {
                    CoderPropID[] propIDs =
                    {
                        CoderPropID.DictionarySize,
                        CoderPropID.PosStateBits,
                        CoderPropID.LitContextBits,
                        CoderPropID.LitPosBits,
                        CoderPropID.Algorithm,
                        CoderPropID.NumFastBytes,
                        CoderPropID.MatchFinder,
                        CoderPropID.EndMarker
                    };

                    object[] properties =
                    {
                        Dictionary,
                        PosStateBits,
                        LitContextBits,
                        LitPosBits,
                        Algorithm,
                        NumFastBytes,
                        Mf,
                        Eos
                    };

                    output.Write(Encoding.UTF8.GetBytes("SC"), 0, 2);
                    output.Write(BitConverter.GetBytes(1).Reverse().ToArray(), 0, 4);
                    output.Write(BitConverter.GetBytes(hash.Length).Reverse().ToArray(), 0, 4);
                    output.Write(hash, 0, hash.Length);
                    
                    encoder.SetCoderProperties(propIDs, properties);

                    encoder.WriteCoderProperties(output);
                    output.Write(BitConverter.GetBytes(input.Length).Reverse().ToArray(), 0, 4);

                    encoder.Code(input, output, input.Length, -1, null);
                    output.Flush();
                    output.Dispose();
                }
                input.Dispose();
            }
            File.Delete(file);
        }

        internal static void DecompressCR(string file)
        {
            var clone = file + ".clone";
            File.Copy(file, clone);
            var decoder = new Decoder();
            using (var input = new FileStream(clone, FileMode.Open))
            {
                using (var output = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    var sc = new byte[2];
                    input.Read(sc, 0, 2);

                        var version = new byte[4];
                        input.Read(version, 0, 4);

                        var md5Length = new byte[4];
                        input.Read(md5Length, 0, 4);

                        var md5 = new byte[16];
                        input.Read(md5, 0, 16);

                    var properties = new byte[5];
                    input.Read(properties, 0, 5);

                    var fileLengthBytes = new byte[4];
                    input.Read(fileLengthBytes, 0, 4);
                    var fileLength = BitConverter.ToInt32(fileLengthBytes, 0);

                    decoder.SetDecoderProperties(properties);
                    decoder.Code(input, output, input.Length, fileLength, null);
                    output.Flush();
                    output.Close();
                }
                input.Close();
            }
            File.Delete(clone);
        }

        internal static void DecompressCoC(string file)
        {
            var clone = file + ".clone";
            File.Copy(file, clone);
            var decoder = new Decoder();
            using (var input = new FileStream(clone, FileMode.Open))
            {
                using (var output = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    var properties = new byte[5];
                    input.Read(properties, 0, 5);

                    var fileLengthBytes = new byte[4];
                    input.Read(fileLengthBytes, 0, 4);
                    var fileLength = BitConverter.ToInt32(fileLengthBytes, 0);

                    decoder.SetDecoderProperties(properties);
                    decoder.Code(input, output, input.Length, fileLength, null);
                    output.Flush();
                    output.Close();
                }
                input.Close();
            }
            File.Delete(clone);
        }
    }
}