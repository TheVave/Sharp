
namespace SharpPhysics._2d.ObjectRepresentation
{
	public class Mesh
	{
		/// <summary>
		/// The X points in the mesh with an offset of the position of the object.
		/// </summary>
		public double[] MeshPointsX;

		/// <summary>
		/// The Y points in the mesh with an offset of the position of the object.
		/// </summary>
		public double[] MeshPointsY;

		/// <summary>
		/// The Z points in the mesh with an offset of the position of the object.
		/// </summary>
		public double[] MeshPointsZ;

		/// <summary>
		/// The points that make up the mesh.
		/// </summary>
		public Point[] MeshPoints;

		/// <summary>
		/// The X points in the mesh with, no offset, exactly how the model was first made.
		/// </summary>
		public double[] MeshPointsActualX;

		/// <summary> 
		/// The Y points in the mesh with, no offset, exactly how the model was first made.
		/// </summary>
		public double[] MeshPointsActualY;

		/// <summary>
		/// The Z points in the mesh with, no offset, exactly how the model was first made.
		/// </summary>
		public double[] MeshPointsActualZ;

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
			MeshPointsActualY = xCopyLayer;

			double[] zCopyLayer = new double[MeshPointsX.Length];
			Array.Copy(MeshPointsX, zCopyLayer, MeshPointsZ.Length);
			MeshPointsActualZ = xCopyLayer;
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
		}
	}
}
