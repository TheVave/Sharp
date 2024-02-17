using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics.Network
{
	public struct Packet
	{
		public byte PacketType;
		public short packetDataSize;
		public short extraDataSize;
		public object? packetData;
		public char[] extraData;
	}
}
