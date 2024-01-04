namespace SharpPhysics._2d.ObjectRepresentation
{
	public class Circle
	{
		public Point Center { get; }
		public double Radius { get; }

		public Circle(Point center, double radius)
		{
			Center = center;
			Radius = radius;
		}
		public override string ToString()
		{
			return $"Center: {Center}, Radius: {Radius}";
		}
	}
}
