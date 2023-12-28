

namespace SharpPhysics
{
    public class Mesh
    {
        public double[] MeshPointsX;
        public double[] MeshPointsY;
        public double[] MeshPointsZ;

		public readonly double[] MeshPointsActualX;
		public readonly double[] MeshPointsActualY;
		public readonly double[] MeshPointsActualZ;

		public Mesh(double[] PointsX, double[] PointsY, double[] PointsZ)
        {
            MeshPointsX = PointsX;
            MeshPointsY = PointsY;
            MeshPointsZ = PointsZ;

			MeshPointsActualX = new double[PointsX.Length];
			Array.Copy(PointsX, MeshPointsActualX, PointsX.Length);

			MeshPointsActualY = new double[PointsY.Length];
			Array.Copy(PointsY, MeshPointsActualY, PointsY.Length);

			MeshPointsActualZ = new double[PointsZ.Length];
			Array.Copy(PointsZ, MeshPointsActualZ, PointsZ.Length);
		}

		/// <summary>
		/// modified ToString method
		/// </summary>
		/// <returns>
		/// a list of all of the points in the mesh repersented as x,y,z
		/// </returns>
		public override string ToString()
		{
            string contructString = string.Empty;
			for (int i = 0; i < MeshPointsX.Length; i++)
            {
                contructString += $"{MeshPointsX[i]},{MeshPointsY[i]},{MeshPointsZ[i]}";
            }
            return contructString;
		}

        /// <summary>
        /// Check 
        /// </summary>
        /// <param name="objToCompare"></param>
        /// <returns></returns>
		public virtual bool Equals(Mesh objToCompare)
		{
			return 
                MeshPointsX.Equals(objToCompare.MeshPointsX) &&
                MeshPointsY.Equals(objToCompare.MeshPointsY) &&
                MeshPointsZ.Equals(objToCompare.MeshPointsZ)   ;
		}
    }
}
