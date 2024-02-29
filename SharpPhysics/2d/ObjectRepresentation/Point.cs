using SharpPhysics._2d.ObjectRepresentation.Translation;
using SharpPhysics.Utilities.MathUtils;
using SharpPhysics.Utilities.MISC;
using System.Numerics;

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

		/// <summary>
		/// A point located at 0,0
		/// Used to speed up some operations.
		/// </summary>
		public static readonly Point BlankPoint2d = new Point(0, 0);

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


		public static Point ShallowCopyPoint(Point pnt) =>
			(pnt.Is3d) ? new(pnt.X, pnt.Y, pnt.Z) : new(pnt.X, pnt.Y);

		public static float[] ToFloatArray3D(Point pnt)
		{
			return [(float)pnt.X, (float)pnt.Y, (float)pnt.Z];
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
		/// internal no check add for speed
		/// </summary>
		/// <param name="p"></param>
		internal static void AddNoCheck2D(Point a, Point b)
		{
			a.X += b.X;
			a.Y += b.Y;
		}
		internal static void AddNoCheck2D(Point a, _2dPosition b)
		{
			a.X += b.X;
			a.Y += b.Y;
		}

		/// <summary>
		/// Adds two Points.
		/// X = X + p.x
		/// Y = Y + p.y
		/// </summary>
		/// <param name="p"></param>
		// this includes some checks
		public void Add(Point p)
		{
			if (p == null) throw new InvalidOperationException("Cannot add point to null");
			if (ContainsValue(p, double.NaN)) throw new InvalidOperationException("Point had a value of NaN");
			if (p.Is3d != this.Is3d) throw new InvalidOperationException("Unable to add points with non-similar dimensions");
			X += p.X;
			Y += p.Y;
		}

		/// <summary>
		/// Checks if a point contains a value
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool ContainsValue(Point pnt, double value) =>
			value == pnt.X || value == pnt.Y || value == pnt.Z;

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
				if (ContainsValue(p, val) is true) return true;
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
				if (!ContainsValue(p, val)) return true;
			}
			return true;
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
