using Sharp.StrangeDataTypes;

namespace Sharp._2d.Raycasting
{
	public class _2dLineMaker(double xStart, double yStart) : IAny
	{
		/// <summary>
		/// the starting x and y values for the ray
		/// </summary>
		public double xStart = xStart, yStart = yStart;
		/// <summary>
		/// Rotation in radians
		/// </summary>
		public double Rot = 0;
		/// <summary>
		/// the Length of the line
		/// </summary>
		public double Length = 1;

		public _2dLineMaker(double xStart, double yStart, double rot) : this(xStart, yStart)
		{
			Rot = rot;
			Length = 1;
		}

		public _2dLineMaker(double xStart, double yStart, double rot, double length) : this(xStart, yStart, rot)
		{
			Length = length;
		}

		/// <summary>
		/// finds a ray with the specified starting points and returns a Ray to represent the ending position after rotation
		/// </summary>
		/// <returns>
		/// a Ray to represent the ending position after rotation
		/// </returns>
		public _2dLine Get_2DRay()
		{
			// create object
			_2dLine _2DRay = new(Rot);
			// set starts.
			_2DRay.YStart = yStart;
			_2DRay.XStart = xStart;
			// the normal rotation function for finding x is <PreviousX>*cos(<theta>)-<PreviousY>*sin(<theta>) but because PreviousX = 0 then it can be just <PreviousY>*sin(<theta>).
			_2DRay.XEnd = (-Length) * Math.Sin(Rot);
			// similar idea to the previous line
			_2DRay.YEnd = Length * Math.Cos(Rot);
			return _2DRay;
		}
	}
}
