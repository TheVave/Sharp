﻿using SharpPhysics._2d.ObjectRepresentation.Translation;
using SharpPhysics._2d.Raycasting;
using SharpPhysics.StrangeDataTypes;

namespace SharpPhysics.Utilities.MathUtils
{
	public class Angle : IAny
	{
		/// <summary>
		/// Rotation in radians
		/// </summary>
		public double AngleValue;

		public Angle(double angleValue)
		{
			AngleValue = angleValue;
		}

		/// <summary>
		/// gets a Translation._2dPosition object that repersents the given paramenters, simlar to cartesian coords
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public Position GetPosition(int length)
		{
			_2dLine line = new _2dLineMaker(0, 0, length, AngleValue).Get_2DRay();
			return new(line.XEnd, line.YEnd);
		}
	}
}
