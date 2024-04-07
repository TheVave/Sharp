using SharpPhysics._2d.Objects;
using SharpPhysics.Utilities.MathUtils.DelaunayTriangulator;
using SharpPhysics.Utilities.MISC.Unsafe;

namespace SharpPhysics._2d.ObjectRepresentation
{
	/// <summary>
	/// The class for holding info on the shape of an object.
	/// 
	/// </summary>
	public class Mesh : ISizeGettable
	{
		/// <summary>
		/// The X points in the mesh with an offset of the position of the object.
		/// </summary>
		public double[] MeshPointsX { get; internal set; }

		/// <summary>
		/// The Y points in the mesh with an offset of the position of the object.
		/// </summary>
		public double[] MeshPointsY { get; internal set; }

		/// <summary>
		/// The Z points in the mesh with an offset of the position of the object.
		/// </summary>
		public double[] MeshPointsZ { get; internal set; }

		/// <summary>
		/// The points that make up the mesh.
		/// </summary>
		public Point[] MeshPoints { get; internal set; }

		/// <summary>
		/// The X points in the mesh with, no offset, exactly how the model was first made.
		/// </summary>
		public double[] MeshPointsActualX { get; internal set; }

		/// <summary> 
		/// The Y points in the mesh with, no offset, exactly how the model was first made.
		/// </summary>
		public double[] MeshPointsActualY { get; internal set; }

		/// <summary>
		/// The Z points in the mesh with, no offset, exactly how the model was first made.
		/// </summary>
		public double[] MeshPointsActualZ { get; internal set; }

		/// <summary>
		/// The triangles that make up the mesh. Used for collision.
		/// </summary>
		public Triangle[] MeshTriangles { get; internal set; }

		/// <summary>
		/// The triangles that make up the mesh. 
		/// Used for rendering.
		/// Not changed for movement.
		/// </summary>
		public Triangle[] ActualTriangles { get; internal set; }

		/// <summary>
		/// The maximum distance of the origin to mesh
		/// </summary>
		public double MaximumDistanceFromCenter;

		public Mesh()
		{
			Mesh mesh = _2dBaseObjects.LoadSquareMesh();
			MeshPointsX = mesh.MeshPointsX;
			MeshPointsY = mesh.MeshPointsY;
			MeshPointsZ = mesh.MeshPointsZ;

			MeshPoints = mesh.MeshPoints;

			MeshPointsActualX = mesh.MeshPointsX;
			MeshPointsActualY = mesh.MeshPointsY;
			MeshPointsActualZ = mesh.MeshPointsZ;

			ActualTriangles = DelaunayTriangulator.DelaunayTriangulation(MeshPoints).ToArray();
			// if I don't recalc the triangles it'll just make a pointer to ActualTriangles
			MeshTriangles = DelaunayTriangulator.DelaunayTriangulation(MeshPoints).ToArray();
		}

		public Mesh(double[] MeshPointsX, double[] MeshPointsY, double[] MeshPointsZ)
		{
			this.MeshPointsX = MeshPointsX;
			this.MeshPointsY = MeshPointsY;
			this.MeshPointsZ = MeshPointsZ;

			double[] xCopyLayer = new double[MeshPointsX.Length];
			Array.Copy(MeshPointsX, xCopyLayer, MeshPointsX.Length);
			MeshPointsActualX = xCopyLayer;

			double[] yCopyLayer = new double[MeshPointsX.Length];
			Array.Copy(MeshPointsY, yCopyLayer, MeshPointsY.Length);
			MeshPointsActualY = yCopyLayer;

			double[] zCopyLayer = new double[MeshPointsX.Length];
			Array.Copy(MeshPointsX, zCopyLayer, MeshPointsZ.Length);
			MeshPointsActualZ = xCopyLayer;

			MeshPoints = new Point[MeshPointsActualX.Length];
			for (int i = 0; i < MeshPointsActualX.Length; i++)
				MeshPoints[i] = new Point(MeshPointsActualX[i], MeshPointsActualY[i]);

			ActualTriangles = DelaunayTriangulator.DelaunayTriangulation(MeshPoints).ToArray();
			// if I don't recalc the triangles it'll just make a pointer to ActualTriangles
			MeshTriangles = DelaunayTriangulator.DelaunayTriangulation(MeshPoints).ToArray();
		}
		public Mesh(double[] MeshPointsX, double[] MeshPointsY)
		{
			this.MeshPointsX = MeshPointsX;
			this.MeshPointsY = MeshPointsY;

			// for ease of conversion from a 2d mesh to a 3d mesh
			this.MeshPointsZ = new double[MeshPointsX.Length];

			double[] xCopyLayer = new double[MeshPointsX.Length];
			Array.Copy(MeshPointsX, xCopyLayer, MeshPointsX.Length);
			MeshPointsActualX = xCopyLayer;

			double[] yCopyLayer = new double[MeshPointsX.Length];
			Array.Copy(MeshPointsY, yCopyLayer, MeshPointsY.Length);
			MeshPointsActualY = yCopyLayer;

			// for ease of conversion from a 2d mesh to a 3d mesh
			MeshPointsActualZ = new double[MeshPointsY.Length];

			// dealing with the MeshPoints field
			MeshPoints = new Point[MeshPointsX.Length];
			for (int i = 0; i < MeshPointsX.Length; i++)
			{
				MeshPoints[i] = new Point(MeshPointsX[i], MeshPointsY[i]);
			}

			ActualTriangles = DelaunayTriangulator.DelaunayTriangulation(MeshPoints).ToArray();
			// if I don't recalc the triangles it'll just make a pointer to ActualTriangles
			MeshTriangles = DelaunayTriangulator.DelaunayTriangulation(MeshPoints).ToArray();
		}

		public unsafe int GetSize() =>
			// six arrays of same len
			(UnsafeUtils.GetArraySize(MeshPointsX) * 6) +
			// MaximumDistanceFromCenter
			(sizeof(double)) +
			// GetSize method size
			// annoying: class inherits from object, so implement those method sizes
			(UnsafeUtils.PtrSize * 5) +
			// complex: triangles
			(UnsafeUtils.GetArraySize(ActualTriangles) * 2);
	}
}