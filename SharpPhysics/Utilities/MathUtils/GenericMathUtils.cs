using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics._2d.ObjectRepresentation.Translation;
using System;
using System.Linq;
using System.Numerics;

namespace SharpPhysics.Utilities.MathUtils
{
	public static class GenericMathUtils
	{


		/// <summary>
		/// Returns true if the input value is positive. False otherwise.
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static bool IsPositive(double x) => x > 0;

		/// <summary>
		/// Returns the opposite of IsPositive.
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static bool IsNegative(double x) => x < 0;

		/// <summary>
		/// Returns true if the value is zero. False if it is any other value.
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static bool IsZero(double x) => x == 0;

		/// <summary>
		/// Returns true if the value is odd.
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static bool IsOdd(double x) => (x / 2 != Math.Floor(x / 2)) ? true : false;

		/// <summary>
		/// Returns true if the value is even.
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static bool IsEven(double x) => (x / 2 == Math.Floor(x / 2)) ? true : false;

		/// <summary>
		/// Finds the difference from the nearest multiple multipleSource
		/// </summary>
		/// <param name="x"></param>
		/// <param name="multipleSource"></param>
		/// <returns></returns>
		public static double GetDifferenceFromNearestMultiple(double x, double multipleSource) => x - (Math.Floor(x / multipleSource) * multipleSource);

		/// <summary>
		/// Finds the percentage with a value and minimum and maximum.
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public static double GetPercentageOverRange(double min, double max, double val) => ((val - min) / (max - min)) * 100;

		/// <summary>
		/// Finds the value from a percentage with the minimum and maximum.
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public static double GetValueFromPercentage(double min, double max, double val) => ((val * (max - min) / 100) + min);

		/// <summary>
		/// Smoothly clamps.
		/// Example:
		/// min: 0, max: 10, newMin: 0, newMax: 100, val: 3, output: 30
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="newMin"></param>
		/// <param name="newMax"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public static double SmoothClamp(double min, double max, double newMin, double newMax, double val) => GetValueFromPercentage(newMin, newMax, GetPercentageOverRange(min, max, val));

		/// <summary>
		/// Sets a value to the negative value if the value is negative and positive if the value is positive.
		/// Example: valueToSet = -3 setTo = 4 output: -4
		/// Example: valueToSet = 6 setTo = 3 output: 3
		/// </summary>
		/// <param name="valueToSet"></param>
		/// <param name="setTo"></param>
		/// <returns></returns>
		// for cleaning up the code:
		public static double NegativePositiveSet(double valueToSet, double setTo)
		{
			if (IsPositive(valueToSet))
				return setTo;

			else return -setTo;
		}

		/// <summary>
		/// Subtracts to zero and not past it.
		/// Also works for negative values with addition.
		/// Example: a = 8 toSubtract = 10 output = 0
		/// a = -4 toSubtract = 3 output = -1
		/// a = -5 toSubtract = 90 output = 0
		/// </summary>
		/// <param name="a"></param>
		/// <param name="toSubtract"></param>
		/// <returns></returns>
		public static double SubtractToZero(double a, double toSubtract) =>
			(IsNegative(a)) ? ((toSubtract < a) ? 0 : a + toSubtract) : ((toSubtract > a) ? 0 : a - toSubtract);

		// /\ (This is the above functionality in a readable form)
		// ||
		// ||
		// if (isNegative(a)
		//  if (toSubtract < a) return 0;
		//	else return a + tosubtract;
		// else
		//	if (tosubtract > a) return 0;
		//  else return a - tosubtract;

		/// <summary>
		/// Subtracts away from zero
		/// </summary>
		/// <param name="minuend"></param>
		/// <param name="subtrahend"></param>
		/// <returns></returns>
		public static int SubtractAwayFromZero(int minuend, int subtrahend) =>
			(IsNegative(minuend)) ?
				minuend - subtrahend : minuend + subtrahend;

		/// <summary>
		/// The same as the normal int.TryParse, but if it fails returns int.MinValue
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static int TryParse(string str) =>
			(int.TryParse(str, out int result)) ? result : int.MinValue;

		private static int toReturn = 0;

		/// <summary>
		/// Converts an string to an int without length.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		// designed to not be a lot of code.
		public static int ParseStrToInt32(string str)
		{
			toReturn = ((str.StartsWith('-')) ? -0 : 0);
			int multiply_val = 1;
			foreach (char c in str)
				try
				{
					toReturn = SubtractAwayFromZero(toReturn,
					int.Parse(c.ToString()) * multiply_val);
					multiply_val *= 10;
				}
				catch
				{
					break;
				}
			return int.Parse(toReturn.ToString().Reverse().ToArray());
		}

		/// <summary>
		/// Converts degrees to radians
		/// </summary>
		/// <param name="degrees"></param>
		/// <returns></returns>
		public static double DegreesToRadians(double degrees) => degrees / 57.2957795;


		/// <summary>
		/// Converts radians to degrees
		/// </summary>
		/// <param name="radians"></param>
		/// <returns></returns>
		public static double RadiansToDegrees(double radians) => radians * 57.2957795;

		/// <summary>
		/// Gets an angle to point a from point b
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static double GetAngleFromPoints(Point a, Point b)
		{
			double strangeDegrees = RadiansToDegrees(Math.Atan((a.X - b.X) / (a.Y - b.Y)));
			double newDegrees = 0;
			// very hacky ||
			//            ||
			//            \/
			if      (b.X > a.X && b.Y > a.Y) newDegrees = strangeDegrees;
			else if (b.X > a.X && b.Y < a.Y) newDegrees = Math.Abs(strangeDegrees) + 90;
			else if (b.X < a.X && b.Y < a.Y) newDegrees = strangeDegrees + 180;
			else if (b.X < a.X && b.Y > a.Y) newDegrees = Math.Abs(strangeDegrees) + 180;
			return newDegrees;
		}

		public static double GetDistanceFromCenter(Point pnt)
		{
			return Math.Sqrt((pnt.X * pnt.X) + (pnt.Y * pnt.Y));
		}

		/// <summary>
		/// Rotates a point around the origin
		/// </summary>
		/// <param name="current"></param>
		/// <param name="toRotate"></param>
		/// <param name="useRadians"></param>
		/// <returns></returns>
		public static Point RotatePointAroundCenter(Point current, double toRotate) =>
			new((current.X * Math.Cos(toRotate)) - (current.Y * Math.Sin(toRotate)), (current.Y * Math.Cos(toRotate)) + (current.X * Math.Sin(toRotate)));
	}
}
