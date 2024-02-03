using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics._2d.Objects;
using SharpPhysics.Utilities.MathUtils.DelaunayTriangulator;
using System.Numerics;

namespace SharpPhysics._2d._2DSGLRenderer.Main
{
	public class SGLRenderedObject
	{
		public uint BoundVao;
		public uint vbo;

		public SharpPhysics._2d.ObjectRepresentation.Point[] objPoints;
		public Triangle[] triangles;
		

		public SGLRenderedObject() 
		{
			objPoints = _2dBaseObjects.LoadSquareMesh().MeshPoints;
			// triangles are handled in renderer
		}
	}
}
