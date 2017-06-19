using System;
using System.Collections.Generic;
using System.IO;
using CRepublic.Boom.Extensions.List;

namespace CRepublic.Boom.Logic
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