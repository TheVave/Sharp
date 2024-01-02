using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics.Utilities.MathUtils;
using SharpPhysics.Utilities.MISC.DelaunayTriangulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
				for (int i = 0; i < mesh.MeshPoints.Length; i++) mesh.MeshPoints[i] = new Point();

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
	}
}
