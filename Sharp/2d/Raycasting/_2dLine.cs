﻿using Sharp._2d.ObjectRepresentation;
using Sharp.StrangeDataTypes;

namespace Sharp._2d.Raycasting
{
	public class _2dLine : IAny
	{
		/// <summary>
		/// the start of the ray at the X position
		/// </summary>
		public double XStart;

		/// <summary>
		/// The start of the array at the Y position.
		/// </summary>
		public double YStart;

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
		public double XEnd;

		/// <summary>
		/// The YEnd of the line, set from 2dLineMaker class.
		/// </summary>
		public double YEnd;
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
			// Pythagorean theorem
			Length = Math.Sqrt(Math.Pow(Math.Abs(XStart - XEnd), 2) + Math.Pow(Math.Abs(YStart - YEnd), 2d));
		}
		public _2dLine(Point a, Point b)
		{
			new _2dLine(a.X, a.Y, b.X, b.Y);
		}
	}
}
