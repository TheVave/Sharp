using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics._2d.ObjectRepresentation.Translation;
using SharpPhysics.StrangeDataTypes;
using SharpPhysics.Utilities.MISC;
using SharpPhysics.Utilities.MISC.Unsafe;

namespace SharpPhysics.Utilities.MathUtils.DelaunayTriangulator
{
	public class Triangle : ISizeGettable, IAny
	{
		public Point Vertex1 { get; private set; }
		public Point Vertex2 { get; private set; }
		public Point Vertex3 { get; private set; }
		public Edge[] Edges { get; private set; }
		public Circle Circumcircle
		{
			get
			{
				double x1 = Vertex1.X;
				double y1 = Vertex1.Y;
				double x2 = Vertex2.X;
				double y2 = Vertex2.Y;
				double x3 = Vertex3.X;
				double y3 = Vertex3.Y;

				double D = 2 * (x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2));
				double Ux = ((x1 * x1 + y1 * y1) * (y2 - y3) + (x2 * x2 + y2 * y2) * (y3 - y1) + (x3 * x3 + y3 * y3) * (y1 - y2)) / D;
				double Uy = ((x1 * x1 + y1 * y1) * (x3 - x2) + (x2 * x2 + y2 * y2) * (x1 - x3) + (x3 * x3 + y3 * y3) * (x2 - x1)) / D;

				Point center = new(Ux, Uy);
				double radius = Math.Sqrt((x1 - Ux) * (x1 - Ux) + (y1 - Uy) * (y1 - Uy));

				return new Circle(center, radius);
			}
		}
		internal bool IsToBeRemoved = false;
		private static readonly double Epsilon = 1e-6;

		public Triangle(Point vertex1, Point vertex2, Point vertex3, bool calculateEdges = true)
		{
			Vertex1 = vertex1;
			Vertex2 = vertex2;
			Vertex3 = vertex3;

			if (calculateEdges)
			{
				Edges = GetEdges(this);
			}
		}

		public Triangle()
		{
			Vertex1 = new(0, 0);
			Vertex2 = new(0, 0);
			Vertex3 = new(0, 0);

			Edges = GetEdges(this);
		}


		private static Edge[] GetEdges(Triangle tri) =>
			[new Edge(tri.Vertex1, tri.Vertex2), new Edge(tri.Vertex2, tri.Vertex3), new Edge(tri.Vertex3, tri.Vertex1)];

		public static void ShiftTriangle(Point pos, ref Triangle tri)
		{
			tri.Vertex1 += pos;
			tri.Vertex2 += pos;
			tri.Vertex3 += pos;
		}
		public static void ShiftTriangle(Point pos, Triangle tri)
		{
			tri.Vertex1 += pos;
			tri.Vertex2 += pos;
			tri.Vertex3 += pos;
		}
		public static void ShiftTriangle(Position pos, Triangle tri)
		{
			tri.Vertex1.X += pos.X;
			tri.Vertex1.Y += pos.Y;
			tri.Vertex2.X += pos.X;
			tri.Vertex2.Y += pos.Y;
			tri.Vertex3.X += pos.X;
			tri.Vertex3.Y += pos.Y;
		}

		public static void RotateByRadians(double radians, ref Triangle tri)
		{
			if (radians == 0) return;
			tri.Vertex1 = GenericMathUtils.RotatePointAroundCenter(tri.Vertex1, radians);
			tri.Vertex2 = GenericMathUtils.RotatePointAroundCenter(tri.Vertex2, radians);
			tri.Vertex3 = GenericMathUtils.RotatePointAroundCenter(tri.Vertex3, radians);
		}

		public static bool IsPointInsideCircumcircle(Point point, Triangle tri)
		{
			double ax = tri.Vertex1.X - point.X;
			double ay = tri.Vertex1.Y - point.Y;
			double bx = tri.Vertex2.X - point.X;
			double by = tri.Vertex2.Y - point.Y;
			double cx = tri.Vertex3.X - point.X;
			double cy = tri.Vertex3.Y - point.Y;

			double ab = ax * by - ay * bx;
			double bc = bx * cy - by * cx;
			double ca = cx * ay - cy * ax;

			double alift = ax * ax + ay * ay;
			double blift = bx * bx + by * by;
			double clift = cx * cx + cy * cy;

			// Check if point is inside the circumcircle
			return alift * bc + blift * ca + clift * ab > Epsilon;
		}
		public override string ToString() =>
			$"Triangle: ({Vertex1.X}, {Vertex1.Y}), ({Vertex2.X}, {Vertex2.Y}), ({Vertex3.X}, {Vertex3.Y})";

		public static explicit operator Point[](Triangle tri) =>
			[tri.Vertex1, tri.Vertex2, tri.Vertex3];

		public static Triangle operator +(Triangle left, Triangle right) =>
			new(left.Vertex1 + right.Vertex1, left.Vertex2 + right.Vertex2, left.Vertex3 + right.Vertex3);

		public static void ScaleTriangle(double xSca, double ySca, ref Triangle triangle)
		{
			triangle.Vertex1.X = triangle.Vertex1.X * xSca;
			triangle.Vertex2.X = triangle.Vertex2.X * xSca;
			triangle.Vertex3.X = triangle.Vertex3.X * xSca;

			triangle.Vertex1.Y = triangle.Vertex1.Y * ySca;
			triangle.Vertex2.Y = triangle.Vertex2.Y * ySca;
			triangle.Vertex3.Y = triangle.Vertex3.Y * ySca;
		}

		public static double GetArea(Triangle tri) =>
			MeshUtilities.GetArea(tri.Vertex1, tri.Vertex2, tri.Vertex3);

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			Triangle other = (Triangle)obj;

			return (AreVerticesEqual(Vertex1, other.Vertex1) || AreVerticesEqual(Vertex1, other.Vertex2) || AreVerticesEqual(Vertex1, other.Vertex3)) &&
				   (AreVerticesEqual(Vertex2, other.Vertex1) || AreVerticesEqual(Vertex2, other.Vertex2) || AreVerticesEqual(Vertex2, other.Vertex3)) &&
				   (AreVerticesEqual(Vertex3, other.Vertex1) || AreVerticesEqual(Vertex3, other.Vertex2) || AreVerticesEqual(Vertex3, other.Vertex3));
		}

		private static bool AreVerticesEqual(Point v1, Point v2)
		{
			return Math.Abs(v1.X - v2.X) < Epsilon && Math.Abs(v1.Y - v2.Y) < Epsilon;
		}

		/// <summary>
		/// Converts a triangle to a float[]
		/// </summary>
		/// <param name="tri"></param>
		/// <returns></returns>
		// abomination of code.
		public static float[] ToFloats3D(Triangle tri)
		{
			return ArrayUtils.ConcatArray(ArrayUtils.ConcatArray(Point.ToFloatArray3D(tri.Vertex1), Point.ToFloatArray3D(tri.Vertex2)), Point.ToFloatArray3D(tri.Vertex3));
		}
		public static float[] ToFloats3D(Triangle[] tri)
		{
			float[] floats = [];
			for (int i = 0; i < tri.Length; i++)
			{
				floats = ArrayUtils.ConcatArray(floats, ToFloats3D(tri[i]));
			}
			return floats;
		}

		public static Triangle Duplicate(Triangle triangle) =>
			new(Point.ShallowCopyPoint(triangle.Vertex1), Point.ShallowCopyPoint(triangle.Vertex2), Point.ShallowCopyPoint(triangle.Vertex3));

		public static bool HasVertex(Point vertex, Triangle tri)
		{
			if (vertex == null) { return false; }
			if (vertex == tri.Vertex1) return true;
			if (vertex == tri.Vertex2) return true;
			if (vertex == tri.Vertex3) return true;
			return false;
		}

		public int GetSize() =>
			(Vertex1.GetSize() * 3) + Circumcircle.GetSize() + UnsafeUtils.GetArraySize(Edges) + sizeof(bool);
	}
}