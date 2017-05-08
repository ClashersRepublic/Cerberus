using System;
using System.Collections.Generic;
using System.IO;
using BL.Servers.BB.Extensions.List;

namespace BL.Servers.BB.Logic
{
    internal class Home
    {
        internal string Layout;
        internal string Village;

        internal Home(string layout = "layout/playerbase.level")
        {
            this.Layout = layout;
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> data = new List<byte>();

                data.AddInt(28);
                data.AddLong(DateTime.UtcNow.Ticks);
                data.AddString(Layout);
                data.AddCompressed(Village);
                return data.ToArray();
            }
        }

    }
}