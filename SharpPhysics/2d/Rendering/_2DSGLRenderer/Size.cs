using SharpPhysics.StrangeDataTypes;

namespace SharpPhysics._2d._2DSGLRenderer
{
	public class Size : IAny
	{
		public int Width;
		public int Height;

		public Size(int width, int height)
		{
			Width = width;
			Height = height;
		}
	}
}
