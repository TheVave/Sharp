using System;
using SharpPhysics;

namespace SharpPhysics
{
	public class _2dLineMaker
	{
		/// <summary>
		/// the starting x and y values for the ray (do not change from RayMaker to Ray)
		/// </summary>
		public double xStart, yStart;
		/// <summary>
		/// Rotation in radians
		/// </summary>
		public double Rot;
		/// <summary>
		/// the Length of the line
		/// </summary>
		public double Length;

		/// <summary>
		/// finds a ray with the spesified starting points and returns a Ray to repersent the ending position after rotation
		/// </summary>
		/// <returns>
		/// a Ray to repersent the ending position after rotation
		/// </returns>
		public _2dLine Get_2DRay()
		{
			// create object
			_2dLine _2DRay = new(Rot);
			// set starts.
			_2DRay.RayData.yStart = yStart;
			_2DRay.RayData.xStart = xStart;
			// the normal rotation function for finding x is <PreviusX>*cos(<thata>)-<PreviusY>*sin(<thata>) but because PreviousX = 0 then it can be just <PreviusY>*sin(<thata>).
			_2DRay.RayData.xEnd = (-Length) * Math.Sin(Rot);
			// simlar idea to the previous line
			_2DRay.RayData.yEnd = Length*Math.Cos(Rot);
			return _2DRay;
		}
	}
}
