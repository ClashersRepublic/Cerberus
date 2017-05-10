using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;

namespace BL.Proxy.Lyra
{
    class Keys
    {
        // Constant public key length
        private const int PublicKeyLength = 32;
        private static Dictionary<Game, string> KeyVersions = new Dictionary<Game, string>();

        /// <summary>
        /// The generated private key, according to the modded public key.
        /// </summary>
        public static byte[] GeneratedPrivateKey
            => "1891d401fadb51d25d3a9174d472a9f691a45b974285d47729c45c6538070d85".ToByteArray();

        /// <summary>
        /// The modded public key.
        /// </summary>
        public static byte[] ModdedPublicKey
            => "72f1a4a4c48e44da0c42310f800e96624e6dc6a641a9d41c3b5039d8dfadc27e".ToByteArray();

        /// <summary>
        /// The original, unmodified public key.
        /// Needed for SecretBox.Encrypt() & SecretBox.Decrypt()
        /// </summary>
        public static byte[] OriginalPublicKey = new byte[PublicKeyLength];

        /// <summary>
        /// Sets the original public key
        /// </summary>
        public static void SetPublicKey()
        {
            try
            {
                // Add Versions
                KeyVersions.Add(Game.CLASH_OF_CLANS, "8.551.4");
                KeyVersions.Add(Game.BOOM_BEACH, "27.136");
                KeyVersions.Add(Game.CLASH_ROYALE, "1.5.0");

                // Set public key
                switch (Config.Game)
                {
                    case Game.CLASH_OF_CLANS:
                        OriginalPublicKey = "349ce78b78a06a4e94645435acba1dfffc40cc2276558ed2d118f1343a197876".ToByteArray();
                        break;
                    case Game.BOOM_BEACH:
                        OriginalPublicKey = "3bf256f1c9457f4465625dba145f2ba2f65b64338351590e796e8119e648755d".ToByteArray();
                        break;
                    case Game.CLASH_ROYALE:
                        OriginalPublicKey = "bbdba8653396d1df84efaea923ecd150d15eb526a46a6c39b53dac974fff3829".ToByteArray();
                        break;
                    default:
                        throw new InvalidPublicKeyException("Unknown game, no public key available..");
                        break;
                }
                Logger.Log("Public key set [" + OriginalPublicKey.ToHexString() + "]");

                /* Compare Versions
                Logger.Log("Comparing public keys..");

                var API_URL = "http://carreto.pt/tools/android-store-version/?package=com.supercell." +
                               Config.Game.ToString().Replace("_", "").ToLower();
                JObject obj = JObject.Parse(new WebClient().DownloadString(API_URL));
                string Version = (string)obj["version"];

                if (Version.Equals(KeyVersions[Config.Game]))
                    Logger.Log("The configured public key is up-to-date.");
                else
                    Logger.Log("The configured public key is OUTDATED! Create a GitHub issue.", LogType.WARNING);*/
            }
            catch(Exception ex)
            {
                Logger.Log("Failed to set/compare publickey (" + ex.GetType() + ")", LogType.EXCEPTION);
            }
        }
    }
}
