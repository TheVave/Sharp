using SharpPhysics.Utilities.MISC;
using System.Reflection.Metadata.Ecma335;

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
		public override string ToString() => (Is3d) ? $"({X},{Y},{zPos})" : $"({X},{Y})";

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
		public Point()
		{
			X = 0;
			Y = 0;
		}

		public float[] ToFloatArray3D()
		{
			return [(float)X, (float)Y, (float)zPos];
		}

		public static implicit operator float[](Point p)
		{
			return [(float)p.X, (float)p.Y];
		}

		public static Point? operator /(Point p1, Point p2) =>
			(p1.Is3d == p2.Is3d) ? ((p1.Is3d) ? new Point(p1.X / p2.X, p1.Y / p2.Y, p1.zPos / p1.zPos) : new Point(p1.X / p2.X, p1.Y / p2.Y)) : null;
		public static Point? operator /(Point p1, double scale) =>
			(p1.Is3d) ? new Point(p1.X / scale, p1.Y / scale, p1.zPos / scale) : new Point(p1.X / scale, p1.Y);
		public static Point operator *(Point p1, double scale) =>
			(p1.Is3d) ? new Point(p1.X * scale, p1.Y * scale, p1.zPos * scale) : new Point(p1.X * scale, p1.Y);

		/// <summary>
		/// Warning! Slow!
		/// </summary>
		public static float[] ToFloatArray(Point[] points)
		{
			float[] toReturn = [];
			ParallelFor.ParallelForLoop((int loopIdx) =>
			{
				toReturn = toReturn.Concat((float[])points[loopIdx]).ToArray();
			}, points.Length * 2);
			return toReturn;
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
