using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics.Network
{
	public enum PacketType
	{
		None,
		Object,
		ExtraData,
		Close,
		Join,
		BlankServerResponse
	}
}
