using System;
using System.Collections.Generic;
using CRepublic.Royale.Logic.Structure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CRepublic.Royale.Logic
{
    internal class Level
    {
        internal Device Device { get; set; }
        internal Player Avatar { get; set; }

        internal Level()
        {
            this.Avatar = new Player();
        }

        internal Level(long id)
        {
            this.Avatar = new Player(id);
        }
    }
}
