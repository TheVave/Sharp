using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics.Utilities.MISC.DelaunayTriangulator;
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
			List<Triangle> triangles = DelaunayTriangulator.DelaunayTriangulator.DelaunayTriangulation(mesh.MeshPoints);
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
	}
}
