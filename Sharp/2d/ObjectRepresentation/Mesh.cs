using Sharp._2d.Objects;
using Sharp.StrangeDataTypes;
using Sharp.Utilities;
using Sharp.Utilities.MathUtils.DelaunayTriangulator;
using Sharp.Utilities.MISC.Unsafe;
using System.Runtime.InteropServices;

namespace Sharp._2d.ObjectRepresentation
{
	/// <summary>
	/// The class for holding info on the shape of an object.
	/// 
	/// </summary>
	public class Mesh : ISizeGettable, IAny
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
		/// Not changed for movement.
		/// </summary>
		public Point[] MeshActualPoints { get; internal set; }

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
		public Triangle[] MeshTriangles 
		{ 
			get 
			{
				return PtrTriangles.Value.Objects;
			} 
			internal set 
			{
				unsafe
				{
					if (PtrTriangles.ObjectPtr == Utils.NULLVOIDPTR)
						PtrTriangles.Create(new TriangleArray(value));
					else
						PtrTriangles.Value = new(value);
				}
			}
		}


		internal UnmanagedMemoryObject<TriangleArray> PtrTriangles = new();

		/// <summary>
		/// The triangles that make up the mesh. 
		/// Used for rendering.
		/// Not changed for movement.
		/// </summary>
		public Triangle[] ActualTriangles { 
			get 
			{
				return PtrActualTriangles.Value.Objects;
			}
			internal set
			{
				unsafe
				{
					if (PtrActualTriangles.ObjectPtr == Utils.NULLVOIDPTR)
						PtrActualTriangles.Create(new TriangleArray(value));
					else
						PtrActualTriangles.Value = new(value);
				}
			}
		}


		internal UnmanagedMemoryObject<TriangleArray> PtrActualTriangles = new();

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
			MeshActualPoints = Point.ShallowCopyPoints(mesh.MeshPoints);

			MeshPointsActualX = mesh.MeshPointsX;
			MeshPointsActualY = mesh.MeshPointsY;
			MeshPointsActualZ = mesh.MeshPointsZ;

			PtrActualTriangles.Create(new TriangleArray([.. DelaunayTriangulator.DelaunayTriangulation(MeshPoints)]));
			// if I don't recalc the triangles it'll just make a pointer to ActualTriangles
			PtrTriangles.Create(new TriangleArray([.. DelaunayTriangulator.DelaunayTriangulation(MeshPoints)]));
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

			Triangle[] preTris = [.. DelaunayTriangulator.DelaunayTriangulation(MeshPoints)];

			TriangleArray tris = new(preTris);

			PtrActualTriangles.Create(tris);

			PtrTriangles.Create(tris);
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

			PtrActualTriangles.Create(new TriangleArray([.. DelaunayTriangulator.DelaunayTriangulation(MeshPoints)]));
			// if I don't recalc the triangles it'll just make a pointer to ActualTriangles
			PtrTriangles.Create(new TriangleArray([.. DelaunayTriangulator.DelaunayTriangulation(MeshPoints)]));
		}

		public unsafe int GetSize() =>
			// six arrays of same len
			(UnsafeUtils.GetArraySize(MeshPointsX) * 6) +
			// points
			(UnsafeUtils.GetArraySize(MeshPoints) * 2) +
			// MaximumDistanceFromCenter
			(sizeof(double)) +
			// complex: triangles
			(PtrTriangles.GetSize() * 2);
		public override string ToString()
		{
			return $"Mesh";
		}
	}
}