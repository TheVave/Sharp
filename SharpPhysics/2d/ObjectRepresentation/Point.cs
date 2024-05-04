using SharpPhysics._2d.ObjectRepresentation.Translation;
using SharpPhysics.StrangeDataTypes;
using SharpPhysics.Utilities;
using SharpPhysics.Utilities.MathUtils;
using SharpPhysics.Utilities.MISC.Unsafe;

namespace SharpPhysics._2d.ObjectRepresentation
{
	public class Point : ISizeGettable, IAny
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
		public Point ShallowCopyPoint() =>
			(Is3d) ? new(X, Y, Z) : new(X, Y);

		public static float[] ToFloatArray3D(Point pnt)
		{
			return [(float)pnt.X, (float)pnt.Y, (float)pnt.Z];
		}
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
		/// internal no check add for speed
		/// </summary>
		/// <param name="p"></param>
		internal static void AddNoCheck2D(Point a, Point b)
		{
			a.X += b.X;
			a.Y += b.Y;
		}
		internal static void AddNoCheck2D(Point a, Position b)
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
		public static void Add(Point p, Point p2)
		{
			if (p == null) throw new InvalidOperationException("Cannot add point to null");
			if (ContainsValue(p, double.NaN)) throw new InvalidOperationException("Point had a value of NaN");
			if (p.Is3d != p2.Is3d) throw new InvalidOperationException("Unable to add points with non-similar dimensions");
			p2.X += p.X;
			p2.Y += p.Y;
		}
		public void Add(Point ToAdd)
		{
			if (this == null) throw new ArgumentNullException("Cannot add point to null");
			if (ContainsValue(this, double.NaN)) throw new InvalidOperationException("Point had a value of NaN");
			if (Is3d != ToAdd.Is3d) throw new InvalidOperationException("Unable to add points with non-similar dimensions");
			X += ToAdd.X;
			Y += ToAdd.Y;
		}

		/// <summary>
		/// Copies a Point[].
		/// </summary>
		/// <param name="pts"></param>
		/// <returns></returns>
		public static Point[] ShallowCopyPoints(Point[] pts)
		{
			Point[] toReturn = new Point[pts.Length];
			for (int i = 0; i < pts.Length; i++)
				toReturn[i] = Point.ShallowCopyPoint(pts[i]);
			return [];
		}

		/// <summary>
		/// Checks if a point contains a value
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool ContainsValue(Point pnt, double value) =>
			value == pnt.X || value == pnt.Y || value == pnt.Z;
		public bool ContainsValue(double value) =>
			value == X || value == Y || value == Z;

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
		/// Converts a Point[] to a float[].
		/// Used for rendering.
		/// </summary>
		// NEW CHANGE: this method's speed has been increased.
		// It is now quite fast.
		public static float[] ToFloatArray(Point[] points)
		{
			float[] toReturn = new float[points.Length * 3];
			for (int i = 0; i < points.Length; i++)
				Array.Copy(points[i].ToFloatArray3D(), 0, toReturn, i * 3, 3);
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

		public int GetSize()
		{
			unsafe
			{
				return sizeof(Point);
			}
		}
	}
}
