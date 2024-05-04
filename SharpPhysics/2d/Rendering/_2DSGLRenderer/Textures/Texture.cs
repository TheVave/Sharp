using SharpPhysics.StrangeDataTypes;

namespace SharpPhysics.Renderer.Textures
{
	public class Texture : IAny
	{
		public byte[] ImageBytes;
		public short ImageWidth;
		public short ImageHeight;
		public bool ContainsAlphaInfo = false;
	}
}
