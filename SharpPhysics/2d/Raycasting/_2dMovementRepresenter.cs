namespace SharpPhysics._2d.Raycasting
{
	public class _2dMovementRepresenter
	{
		public double xStart = 0;
		public double yStart = 0;
		public double xEnd = 0;
		public double yEnd = 0;
		public _2d.ObjectRepresentation.Point Start;
		public _2d.ObjectRepresentation.Point End;

		public _2dMovementRepresenter(double xStart, double yStart, double xEnd, double yEnd)
		{
			this.xStart = xStart;
			this.yStart = yStart;
			this.xEnd = xEnd;
			this.yEnd = yEnd;
			Start = new(xStart, yStart);
			End = new(xEnd, yEnd);
		}
	}
}
