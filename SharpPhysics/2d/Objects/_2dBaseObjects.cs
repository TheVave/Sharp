using SharpPhysics._2d.ObjectRepresentation;

namespace SharpPhysics._2d.Objects
{
	public static class _2dBaseObjects
	{
		public static Mesh LoadSquareMesh()
		{
			return new(new double[] { 2, -2, -2, 2 }, new double[] { 2, 2, -2, -2 }, new double[] { 0, 0, 0, 0 });
		}
	}
}
