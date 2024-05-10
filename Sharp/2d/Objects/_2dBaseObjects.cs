using Sharp._2d.ObjectRepresentation;
using Sharp.StrangeDataTypes;

namespace Sharp._2d.Objects
{
	public static class _2dBaseObjects
	{
		public static Mesh LoadSquareMesh()
		{
			return new([0.5, -0.5, -0.5, 0.5], [0.5, 0.5, -0.5, -0.5], [0, 0, 0, 0]);
		}
	}
}
