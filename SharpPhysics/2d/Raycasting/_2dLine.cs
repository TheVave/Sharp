using System;
using SharpPhysics;

namespace SharpPhysics
{
	public class _2dLine
	{
		internal _2dMovementRepersenter RayData;
		/// <summary>
		/// the start of the ray at the X position
		/// </summary>
		public double XStart { get { return RayData.xStart; } set { RayData.xStart = value; } }
		/// <summary>
		/// The start of the array at the Y position.
		/// </summary>
		public double YStart { get { return RayData.yStart; } set { RayData.yStart = value; } }

		/// <summary>
		/// the Length of the line, set from 2dLineMaker class.
		/// </summary>
		public double Length { get; set; }

		/// <summary>
		/// Rotation of the line, set from 2dLineMaker class.
		/// </summary>
		public readonly double Rotation = 0;

		/// <summary>
		/// The XEnd of the line, set from 2dLineMaker class
		/// </summary>
		public double XEnd { get { return RayData.xEnd; } set { RayData.xEnd = value; } }

		/// <summary>
		/// The YEnd of the line, set from 2dLineMaker class.
		/// </summary>
		public double YEnd { get { return RayData.yEnd; } set { RayData.yEnd = value; } }

		public bool CheckIfCollided()
		{
			return true;
		}
		internal _2dLine(double rot) { Rotation = rot; }
		public _2dLine(double xEnd, double yEnd)
		{
			XEnd = xEnd;
			YEnd = yEnd;
		}
		public _2dLine(double xStart, double yStart, double xEnd, double yEnd)
		{
			XStart = xStart;
			YStart = yStart;
			XEnd = xEnd;
			YEnd = yEnd;
			// pothag thereom
			Length = Math.Sqrt(Math.Pow(Math.Abs(XStart - XEnd), 2) + Math.Pow(Math.Abs(YStart - YEnd), 2d));
		}
		public _2dLine(Point a, Point b) => new _2dLine(a.X, a.Y, b.X, b.Y);
		public static _2dSimulatedObject? CheckIfRayCollidedWithObject(_2dLine ray)
		{
			return null;
			_2dLineMaker rayMaker = new();
			_2dLine line = new(0);
			rayMaker.xStart = ray.XStart;
			rayMaker.yStart = ray.YStart;
			for (int i = 0; i < (ray.Length * 100); i++)
			{
				rayMaker.Length = i / 100;
				rayMaker.Rot = ray.Rotation;
				line = rayMaker.Get_2DRay();
				for (int j = 0; j++ < SimulationHierarchy.GetObjectCount();) return null;
			}
			
		}
	}
}
