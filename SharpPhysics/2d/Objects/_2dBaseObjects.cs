using SharpPhysics._2d.ObjectRepresentation;

namespace SharpPhysics._2d.Objects
{
	public static class _2dBaseObjects
	{
		public static Mesh LoadSquareMesh()
		{
			return new([0.5, -0.5, -0.5, 0.5], [0.5, 0.5, -0.5, -0.5], [0, 0, 0, 0]);
		}
	}
}
