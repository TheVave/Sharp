using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics.Utilities.MathUtils;
using SharpPhysics.Utilities.MathUtils.DelaunayTriangulator;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SharpPhysics.Utilities.MISC
{
	public static class RenderingUtils
	{

		static int i6 = 0;
		public static float[] MeshToVertices(Mesh mesh)
		{
			if (mesh.MeshPoints is null)
			{
				mesh.MeshPoints = new SharpPhysics._2d.ObjectRepresentation.Point[mesh.MeshPointsActualX.Length];
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

		//Code from: https://stackoverflow.com/questions/18407349/systemdrawingbitmap-to-unsigned-char
		/// <summary>
		/// Converts a bitmap image to a byte[]
		/// </summary>
		/// <param name="bitmap"></param>
		/// <returns></returns>
		public static unsafe byte[] GetImageBytes(Bitmap bitmap)
		{
			Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
			BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadOnly,
				bitmap.PixelFormat);

			IntPtr ptr = bmpData.Scan0;
			int bytes = Math.Abs(bmpData.Stride) * bitmap.Height;
			byte[] rgbValues = new byte[bytes];
			Marshal.Copy(ptr, rgbValues, 0, bytes);
			bitmap.UnlockBits(bmpData);
			return rgbValues;
			//do whatever with data
		}

		/// <summary>
		/// Gets a float[] containing the texture cords
		/// </summary>
		/// <param name="msh"></param>
		/// <returns></returns>
		public static float[] GetTXCords(Mesh msh)
		{
			_2d.ObjectRepresentation.Point[] output = new _2d.ObjectRepresentation.Point[msh.MeshTriangles.Length * 3];
			double maxDist = MeshUtilities.CalculateMaxDistFromCenter(msh, new());
			int idx = 0;
			Triangle triAn;
			foreach (Triangle tri in msh.MeshTriangles)
			{
				triAn = new(tri.Vertex1, tri.Vertex2, tri.Vertex3);
				//triAn.Vertex1 = 
				output[idx    ] = tri.Vertex1 * -1 / maxDist;
				output[idx + 1] = tri.Vertex2 * -1 / maxDist;
				output[idx + 2] = tri.Vertex3 * -1 / maxDist;

				idx += 3;
			}
			return _2d.ObjectRepresentation.Point.ToFloatArray(output);
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
	}
}
