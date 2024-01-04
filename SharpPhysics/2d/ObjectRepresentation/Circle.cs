namespace SharpPhysics._2d.ObjectRepresentation
{
	public class Circle
	{
		public Point Center { get; } = new Point();
		public double Radius { get; } = 0;

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
