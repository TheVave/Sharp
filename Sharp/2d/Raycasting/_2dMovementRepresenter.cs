using Sharp.StrangeDataTypes;

namespace Sharp._2d.Raycasting
{
	public class _2dMovementRepresenter(double xStart, double yStart, double xEnd, double yEnd) : IAny
	{
		public double xStart = xStart;
		public double yStart = yStart;
		public double xEnd = xEnd;
		public double yEnd = yEnd;
		public _2d.ObjectRepresentation.Point Start = new(xStart, yStart);
		public _2d.ObjectRepresentation.Point End = new(xEnd, yEnd);
	}
}
