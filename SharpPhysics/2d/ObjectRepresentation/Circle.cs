using SharpPhysics.StrangeDataTypes;
using SharpPhysics.Utilities.MISC.Unsafe;

namespace SharpPhysics._2d.ObjectRepresentation
{
	public class Circle(Point center, double radius) : ISizeGettable, IAny
	{
		public Point Center { get; } = center;
		public double Radius { get; } = radius;

		public int GetSize()
		{
			return sizeof(double) + Center.GetSize();
		}

		public override string ToString()
		{
			return $"Center: {Center}, Radius: {Radius}";
		}
	}
}
