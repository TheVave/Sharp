using Sharp._2d._2DSGLRenderer.Main;
using Sharp._2d.ObjectRepresentation;
using Sharp.Utilities.MathUtils;
using Sharp.Utilities.MathUtils.DelaunayTriangulator;
using Sharp.Utilities.MISC.Unsafe;
using Silk.NET.Core;
using StbImageSharp;
using System.Runtime.ExceptionServices;
using static Sharp.Utilities.MathUtils.GenericMathUtils;

namespace Sharp.Utilities.MISC
{
	public static class RenderingUtils
	{

		static int i6 = 0;
		public static float[] MeshToVertices(Mesh mesh)
		{
			if (mesh.MeshPoints is null)
			{
				mesh.MeshPoints = new Point[mesh.MeshPointsActualX.Length];
				for (int i = 0; i < mesh.MeshPoints.Length; i++) mesh.MeshPoints[i] = new Sharp._2d.ObjectRepresentation.Point();

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

		public static RawImage GetRawImageFromImageResult(ImageResult reslt) =>
			new(reslt.Width, reslt.Height, new Memory<byte>(reslt.Data));

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


				output[idx + 0] = points[0] * (-1 / maxDist);
				output[idx + 1] = points[1] * (-1 / maxDist);
				output[idx + 2] = points[2] * (-1 / maxDist);

				idx += 3;
			}

			Point ToShiftBy = GetGreatestPointFromArray(shifters);

			for (int i = 0; i < output.Length; i++)
				output[i] += ToShiftBy;

			return Point.ToFloatArray(output, false);
		}

		/// <summary>
		/// Gets the greatest shifter point
		/// </summary>
		/// <param name="arr"></param>
		/// <returns></returns>
		public static Point GetGreatestPointFromArray(Point[] arr)
		{
			Point curGreatest = new(0, 0);
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
			Point curLowest = new(0, 0);
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
			double mthRtrn;

			for (int i = 0; i < toRet.Length; i++)
			{
				mthRtrn = i - (Math.Floor(i / 5d) * 5);
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
		public static uint[] GetEbo(ref float[] vertices)
		{
			Dictionary<string, uint> indexMap = [];
			List<uint> indices = [];

			for (int i = 0; i < vertices.Length; i += 3)
			{
				string vertexKey = $"{vertices[i]},{vertices[i + 1]},{vertices[i + 2]}";
				if (!indexMap.ContainsKey(vertexKey))
				{
					indexMap[vertexKey] = (uint)indexMap.Count;
				}
				indices.Add(indexMap[vertexKey]);
			}

			var distinctVertices = indexMap.Keys.SelectMany(v => v.Split(',').Select(float.Parse)).ToArray();
			for (int i = 0; i < indices.Count; i++)
			{
				vertices[i * 3] = distinctVertices[(int)indices[i] * 3];
				vertices[i * 3 + 1] = distinctVertices[(int)indices[i] * 3 + 1];
				vertices[i * 3 + 2] = distinctVertices[(int)indices[i] * 3 + 2];
			}
			return [.. indices];
		}

		/// <summary>
		/// has a length of one for no OTHER results, assuming that values contains valueToSearch
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="values"></param>
		/// <param name="valueToSearch"></param>
		/// <returns></returns>
		public static int[] AllIndexesOf<T>(T[] values, T valueToSearch)
		{
			Span<int> toReturn = [];
			int idx = 0;
			foreach (T val in values)
			{
				// can't ==
				if (val.Equals(valueToSearch)) ArrayUtils.AddSpanObject(toReturn, idx);
				idx++;
			}
			return toReturn.ToArray();
		}

		/// <summary>
		/// Not recommended to be used directly, as it handles some memory stuff.
		/// Creates a blank sglRenderedObject from a SimulatedObject2d
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static unsafe SGLRenderedObject GetBlankSGLRenderedObjectFromSimulatedObject2d(SimulatedObject2d obj)
		{
			// must be freed later because mem doesn't get collected by the GC
			UnmanagedMemoryObject<SimulatedObject2d> objToSim = new();
			objToSim.Create(obj);
			return new SGLRenderedObject()
			{
				objToSim = objToSim,
				NeedMemFreeSimulatedObject2d = true
			};
		}

		/// <summary>
		/// Not recommended to be used directly, as it handles some memory stuff.
		/// Creates multiple blank sglRenderedObject's from SimulatedObject2d's
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		// may be the longest method name I've ever written
		public static unsafe SGLRenderedObject[] GetBlankSGLRenderedObjectArrayFromSimulatedObject2dArray(SimulatedObject2d[] objs)
		{
			SGLRenderedObject[] valueToReturn = new SGLRenderedObject[objs.Length];
			for (int i = 0; i < valueToReturn.Length; i++)
				valueToReturn[i] = GetBlankSGLRenderedObjectFromSimulatedObject2d(objs[i]);
			return valueToReturn;
		}
	}
}