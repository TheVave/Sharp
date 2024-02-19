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
