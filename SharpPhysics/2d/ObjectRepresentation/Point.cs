using SharpPhysics.Utilities.MathUtils;
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
		public double Z = 0;

		/// <summary>
		/// 
		/// </summary>
		internal bool Is3d = false;
		public override string ToString() => (Is3d) ? $"({X},{Y},{Z})" : $"({X},{Y})";

		internal Point(double xPos, double yPos, double zPos, bool is3d)
		{
			this.X = xPos;
			this.Y = yPos;
			this.Z = zPos;
			Is3d = is3d;
		}
		public Point(double xPos, double yPos, double zPos)
		{
			this.X = xPos;
			this.Y = yPos;
			this.Z = zPos;
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

		public Point GetPointCopy() =>
			(this.Is3d) ? new(X, Y, Z) : new(X, Y);

		public float[] ToFloatArray3D()
		{
			return [(float)X, (float)Y, (float)Z];
		}

		public static implicit operator float[](Point p)
		{
			return [(float)p.X, (float)p.Y];
		}

		public static Point? operator /(Point p1, Point p2) =>
			(p1.Is3d == p2.Is3d) ? ((p1.Is3d) ? new Point(p1.X / p2.X, p1.Y / p2.Y, p1.Z / p1.Z) : new Point(p1.X / p2.X, p1.Y / p2.Y)) : null;
		public static Point? operator /(Point p1, double scale) =>
			(p1.Is3d) ? new Point(p1.X / scale, p1.Y / scale, p1.Z / scale) : new Point(p1.X / scale, p1.Y);
		public static Point operator *(Point p1, double scale) =>
			(p1.Is3d) ? new Point(p1.X * scale, p1.Y * scale, p1.Z * scale) : new Point(p1.X * scale, p1.Y);
		public static Point operator +(Point p1, Point p2) =>
			(p1.Is3d || p2.Is3d) ? new(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z) : new(p1.X + p2.X, p1.Y + p2.Y);
		public static Point operator -(Point p1, Point p2) =>
			(p1.Is3d || p2.Is3d) ? new(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z) : new(p1.X - p2.X, p1.Y - p2.Y);

		/// <summary>
		/// Checks if a point contains a value
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool ContainsValue(double value) =>
			(value == X || value == Y || value == Z) ? false : true;

		/// <summary>
		/// Checks if a point array contains a value
		/// </summary>
		/// <param name="points"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public static bool ArrayContainsValue(Point[] points, double val)
		{
			foreach (Point p in points)
			{
				if (p.ContainsValue(val) is true) return true;
			}
			return false;
 		}

		/// <summary>
		/// The opposite of ArrayContainsValue
		/// </summary>
		/// <param name="points"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public static bool ArrayNotContainValue(Point[] points, double val)
		{
			foreach (Point p in points)
			{
				if (!p.ContainsValue(val)) return true;
			}
			return false;
		}
			

		/// <summary>
		/// Warning! Slow!
		/// </summary>
		public static float[] ToFloatArray(Point[] points)
		{
			float[] toReturn = [];
			ParallelFor.ParallelForLoop((int loopIdx) =>
			{
				toReturn = toReturn.Concat((float[])(points[loopIdx])).ToArray();
			}, points.Length);
			return toReturn;
		}

		public static bool HasNegative(Point pnt) =>
			(GenericMathUtils.IsNegative(pnt.X) || GenericMathUtils.IsNegative(pnt.Y) || GenericMathUtils.IsNegative(pnt.Z)) ? true : false;
		public static bool HasPositive(Point pnt) =>
			!HasNegative(pnt);

		public static Point Rotate2dPoint(double radians, Point pnt)
		{
			Point resltpnt = pnt;
			resltpnt.X = pnt.X * Math.Cos(radians) - pnt.Y * Math.Cos(radians);
			resltpnt.Y = pnt.X * Math.Sin(radians) + pnt.Y * Math.Cos(radians);
			return resltpnt;
		}
	}
}
