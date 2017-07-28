namespace CR.Assets.Editor.Compression
{

    using System.IO;
    using System.Runtime.InteropServices;
    internal class Lzham
    {

        [DllImport(@"Library\SupercellUtil.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool Decompress([MarshalAs(UnmanagedType.LPStr)] string path);

        [DllImport(@"Library\SupercellUtil.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool Compress([MarshalAs(UnmanagedType.LPStr)] string path); //Buggy


        internal static void DecompressSc(string file)
        {
            if (Decompress(file))
            {
                var clone = file + ".clone";
                File.Copy(file, clone);

                using (var input = new FileStream(clone, FileMode.Open))
                {
                    using (var output = new FileStream(file, FileMode.Create, FileAccess.Write))
                    {

                        var decompresserbug = new byte[35]; //Won't be needed when I get dll fix from ToxicOverflow
                        input.Read(decompresserbug, 0, 35);

                        input.CopyTo(output);
                        output.Flush();
                        output.Close();
                    }
                    input.Close();
                }
                File.Delete(clone);
            }
        }
    }
}
