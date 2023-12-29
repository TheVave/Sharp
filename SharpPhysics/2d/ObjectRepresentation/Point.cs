namespace SharpPhysics._2d.ObjectRepresentation
{
	public class Point
	{
		/// <summary>
		/// The X position of the point
		/// </summary>
		public double X = 0;

		/// <summary>
		/// The Y position of the point
		/// </summary>
		public double Y = 0;

		/// <summary>
		/// The Z position of the point
		/// </summary>
		public double zPos = 0;

		/// <summary>
		/// 
		/// </summary>
		internal bool Is3d = false;
		internal Point(double xPos, double yPos, double zPos, bool is3d)
		{
			this.X = xPos;
			this.Y = yPos;
			this.zPos = zPos;
			Is3d = is3d;
		}
		public Point(double xPos, double yPos, double zPos)
		{
			this.X = xPos;
			this.Y = yPos;
			this.zPos = zPos;
			Is3d = true;
		}
		public Point(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		public static Point Rotate2dPoint(double radians, Point pnt)
		{
			Point resltpnt = pnt;
			resltpnt.X = pnt.X * Math.Cos(radians) - pnt.Y * Math.Cos(radians);
			resltpnt.Y = pnt.X * Math.Sin(radians) + pnt.Y * Math.Cos(radians);
			return resltpnt;
		}
	}
}
