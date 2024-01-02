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
		public static double Clamp(double min, double max, double newMin, double newMax, double val) => GetValueFromPercentage(newMin, newMax, GetPercentageOverRange(min, max, val));

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
		public static double SubtractToZero(double a, double toSubtract)
		{
			if (IsNegative(a))
			{
				if (toSubtract < a) return 0;
				else return a + toSubtract;
			}
			else /* if (IsPositive(a)) */
			{
				if (toSubtract > a) return 0;
				else return a - toSubtract;
			}
		}
	}
}
