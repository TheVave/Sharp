using SharpPhysics._2d._2DSGLRenderer.Main;
using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics.Utilities.MathUtils;
using SharpPhysics.Utilities.MathUtils.DelaunayTriangulator;
using Silk.NET.Core;
using StbImageSharp;
using static SharpPhysics.Utilities.MathUtils.GenericMathUtils;

namespace SharpPhysics.Utilities.MISC
{
	public static class RenderingUtils
	{

		static int i6 = 0;
		public static float[] MeshToVertices(Mesh mesh)
		{
			if (mesh.MeshPoints is null)
			{
				mesh.MeshPoints = new Point[mesh.MeshPointsActualX.Length];
				for (int i = 0; i < mesh.MeshPoints.Length; i++) mesh.MeshPoints[i] = new SharpPhysics._2d.ObjectRepresentation.Point();

				for (int i = 0; i < mesh.MeshPointsActualX.Length; i++)
				{
					mesh.MeshPoints[i].X = mesh.MeshPointsX[i];
					mesh.MeshPoints[i].Y = mesh.MeshPointsY[i];
				}
			}
			List<Triangle> triangles = DelaunayTriangulator.DelaunayTriangulation(mesh.MeshPoints);
			float[] vertices = new float[triangles.Count * 6]; // Each triangle has 3 vertices with 2 coordinates each
			i6 = 0;
			for (int i = 0; i < triangles.Count; i++)
			{
				vertices[i6] = (float)triangles[i].Vertex1.X;
				vertices[i6 + 1] = (float)triangles[i].Vertex1.Y;

				vertices[i6 + 2] = (float)triangles[i].Vertex2.X;
				vertices[i6 + 3] = (float)triangles[i].Vertex2.Y;

				vertices[i6 + 4] = (float)triangles[i].Vertex3.X;
				vertices[i6 + 5] = (float)triangles[i].Vertex3.Y;
				i6 += 6;
			}


			return vertices;
		}

		public static void SetTX(ref SGLRenderedObject obj)
		{

		}

		public static RawImage GetRawImageFromImageResult(ImageResult reslt) =>
			new RawImage(reslt.Width, reslt.Height, new Memory<byte>(reslt.Data));

		/// <summary>
		/// Gets a float[] containing the texture cords
		/// </summary>
		/// <param name="msh"></param>
		/// <returns></returns>
		public static float[] GetTXCords(Mesh msh)
		{
			Point[] output = new Point[msh.MeshTriangles.Length * 3];
			double maxDist = MeshUtilities.CalculateMaxDistFromCenter(msh, new());
			int idx = 0;
			Point[] points = new Point[3];
			Point[] shifters = new Point[3 * msh.MeshTriangles.Length];
			foreach (Triangle tri in msh.MeshTriangles)
			{
				points = [new(tri.Vertex1.X, tri.Vertex1.Y), new(tri.Vertex2.X, tri.Vertex2.Y), new(tri.Vertex3.X, tri.Vertex3.Y)];

				shifters[idx + 0] = new((IsNegative(tri.Vertex1.X)) ? Math.Abs(tri.Vertex1.X) : 0, (IsNegative(tri.Vertex1.Y)) ? Math.Abs(tri.Vertex1.Y) : 0);
				shifters[idx + 1] = new((IsNegative(tri.Vertex2.X)) ? Math.Abs(tri.Vertex2.X) : 0, (IsNegative(tri.Vertex2.Y)) ? Math.Abs(tri.Vertex2.Y) : 0);
				shifters[idx + 2] = new((IsNegative(tri.Vertex3.X)) ? Math.Abs(tri.Vertex3.X) : 0, (IsNegative(tri.Vertex3.Y)) ? Math.Abs(tri.Vertex3.Y) : 0);
				

				output[idx + 0]  = points[0] * (-1 / maxDist);
				output[idx + 1]  = points[1] * (-1 / maxDist);
				output[idx + 2]  = points[2] * (-1 / maxDist);

				idx += 3;
			}

			Point ToShiftBy = GetGreatestPointFromArray(shifters);

			for (int i = 0; i < output.Length; i++)
				output[i] += ToShiftBy;

			return Point.ToFloatArray(output);
		}

		/// <summary>
		/// Gets the greatest shifter point
		/// </summary>
		/// <param name="arr"></param>
		/// <returns></returns>
		public static Point GetGreatestPointFromArray(Point[] arr)
		{
			Point curGreatest = new Point(0,0);
			foreach (Point p in arr)
			{
				if (p.X > curGreatest.X) curGreatest.X = p.X;
				if (p.Y > curGreatest.Y) curGreatest.Y = p.Y;
			}
			return curGreatest;
		}

		/// <summary>
		/// Gets the lowest shifter point
		/// </summary>
		/// <param name="arr"></param>
		/// <returns></returns>
		public static Point GetLowestPointFromArray(Point[] arr)
		{
			Point curLowest = new Point(0, 0);
			foreach (Point p in arr)
			{
				if (p.X < curLowest.X) curLowest.X = p.X;
				if (p.Y < curLowest.Y) curLowest.Y = p.Y;
			}
			return curLowest;
		}

		/// <summary>
		/// Style:
		/// posCords[3] TxCords[2]
		/// [0,0,0],[1,0]
		/// </summary>
		/// <param name="first"></param>
		/// <param name="last"></param>
		/// <param name="MashEvery"></param>
		/// <returns></returns>
		public static float[] MashMeshTextureFloats(float[] first, float[] last)
		{
			float[] toRet = new float[first.Length + last.Length];
			int firstIdx = 0;
			int lastIdx = 0;
			int mthRtrn;

			for (int i = 0; i < toRet.Length; i++)
			{
				mthRtrn = i - (int)(Math.Floor(i / 5d) * 5);
				if (mthRtrn <= 2)
					toRet[i] = first[firstIdx++];
				else
					toRet[i] = -last[lastIdx++];
			}
			return toRet;
		}

		/// <summary>
		/// Collects ebo data
		/// </summary>
		/// <param name="vbo"></param>
		/// <returns></returns>
		public static uint[] GetEbo(uint[] vbo)
		{
			return [];
		}
	}
}