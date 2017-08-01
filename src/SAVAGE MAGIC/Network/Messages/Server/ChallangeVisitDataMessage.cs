using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network;

namespace Magic.Packets.Messages.Server
{
	internal class ChallangeVisitDataMessage : Message
	{
		public ChallangeVisitDataMessage(ClashOfClans.Client client, Level level) : base(client)
		{
			MessageType = 25007;
		}

		public override void Encode()
		{
		}

		public override void Process(Level level)
		{
			var list = new List<byte>();
			Encrypt(list.ToArray());
		}
	}
}
