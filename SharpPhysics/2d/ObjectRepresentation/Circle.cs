namespace SharpPhysics._2d.ObjectRepresentation
{
	public class Circle(Point center, double radius)
	{
		public Point Center { get; } = center;
		public double Radius { get; } = radius;

		public override string ToString()
		{
			return $"Center: {Center}, Radius: {Radius}";
		}
	}
}
